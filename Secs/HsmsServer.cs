using Secs.Enums;
using Secs.Exceptions;
using Secs.Extensions;
using Secs.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;

namespace Secs
{
    public class HsmsServer
    {
        private Socket? _callerSocket;
        private Socket? _server;
        private CancellationTokenSource? _loopReceiveTokenSource;
        private CancellationTokenSource? _loopProcessSecondaryContextsTokenSource;
        private CancellationTokenSource? _loopLinkTestTokenSource;
        private CancellationTokenSource? _acceptTokenSource;
        /// <summary>
        /// Reply-Required Message
        /// </summary>
        public const byte Reply = 0x80;
        /// <summary>
        /// PType,default:0
        /// </summary>
        public const byte PType = 0;
        /// <summary>
        /// Default Equipment ID
        /// </summary>
        public const ushort DefaultSessionId = 0xFFFF;
        private ConnectionState connectionState;
        public ConnectionState ConnectionState
        {
            get { return connectionState; }
            set
            {
                if (value != connectionState)
                {
                    ConnectionStateChanged?.Invoke(value);
                }
                connectionState = value;
            }
        }
        private readonly Dictionary<int, HsmsDataContext> _secondaryContext = new();
        private readonly Dictionary<int, HsmsDataContext> _primaryContext = new();
        private readonly SemaphoreSlim _secondaryContextSemaphoreSlim = new(1, 1);
        private readonly SemaphoreSlim _primaryContextSemaphoreSlim = new(1, 1);
        public int SecondaryContextCount => _secondaryContext.Count;
        public int PrimaryContextCount => _primaryContext.Count;
        public EndPoint? LocalEndPoint { get; private set; }
        public EndPoint? RemoteEndPoint { get; private set; }
        /// <summary>
        /// Hsms server options
        /// </summary>
        public HsmsOptions Options { get; }
        public bool ServerStart { get; private set; }
        /// <summary>
        /// Connection session changed
        /// </summary>
        public Action<EndPoint?, EndPoint?, bool>? SessionConnectionChanged;
        /// <summary>
        /// Internal exception
        /// </summary>
        public Action<string?, Exception>? InternalException;
        /// <summary>
        /// Record send or receive raw message
        /// </summary>
        public Action<byte[], RawType>? RawMessageChanged;
        /// <summary>
        /// Hsms data context changed
        /// </summary>
        public Action<HsmsDataContext>? HsmsDataContextChanged;
        /// <summary>
        /// Record send or receive hsms message
        /// </summary>
        public Action<HsmsMessage>? HsmsMessageChanged;
        /// <summary>
        /// connection state changed
        /// </summary>
        public Action<ConnectionState>? ConnectionStateChanged;
        /// <summary>
        /// Subscribe passive remote caller hsms message
        /// </summary>
        public Func<HsmsMessage, HsmsMessage?>? SubscribeRemoteCaller;
        public HsmsServer(HsmsOptions options)
        {
            Options = options;
        }
        public void Start()
        {
            if (Options.ConnectionMode == ConnectionMode.Active)
            {
                StartActive();
            }
            else
            {
                StartPassive();
            }
        }
        public void Stop()
        {
            if (Options.ConnectionMode == ConnectionMode.Active)
            {
                StopActive();
            }
            else
            {
                StopPassive();
            }
        }
        private void StartActive()
        {
            if (ServerStart) return;

            var ep = new IPEndPoint(IPAddress.Parse(Options.IP), Options.Port);
            _callerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ConnectionState = ConnectionState.NotConnected;

            _callerSocket.Connect(ep);
            LocalEndPoint = _callerSocket.LocalEndPoint;
            RemoteEndPoint = _callerSocket.RemoteEndPoint;
            ConnectionState = ConnectionState.Connected;
            SessionConnectionChanged?.Invoke(LocalEndPoint, RemoteEndPoint, true);
            ConnectionState = ConnectionState.NotSelected;

            Task.Run(LoopReceiveAsync);
            _ = LoopProcessSecondaryContextsAsync();
            SendSelectReq();
            ConnectionState = ConnectionState.Selected;

            if (Options.LinkTest)
            {
                _ = LoopSendLinkTestAsync();
            }
            ServerStart = true;
        }
        private void StopActive()
        {
            try
            {
                if (_callerSocket != null && _callerSocket.Connected)
                {
                    SendSeparateReq();
                    ConnectionState = ConnectionState.NotSelected;
                }
                _loopLinkTestTokenSource.Destroy();
                _loopReceiveTokenSource.Destroy();
                _loopProcessSecondaryContextsTokenSource.Destroy();
                _callerSocket.SafeClose();
                ConnectionState = ConnectionState.NotConnected;
                SessionConnectionChanged?.Invoke(LocalEndPoint, RemoteEndPoint, false);
            }
            finally
            {
                ServerStart = false;
            }
        }
        private void StartPassive()
        {
            if (ServerStart) return;

            _ = ListenAsync();
            _ = LoopProcessSecondaryContextsAsync();
            if (Options.LinkTest)
            {
                _ = LoopSendLinkTestAsync();
            }
            ServerStart = true;
        }
        private void StopPassive()
        {
            try
            {
                _loopLinkTestTokenSource.Destroy();
                _loopReceiveTokenSource.Destroy();
                _loopProcessSecondaryContextsTokenSource.Destroy();
                _server.SafeClose();
                _callerSocket.SafeClose();
                ConnectionState = ConnectionState.NotConnected;
                SessionConnectionChanged?.Invoke(LocalEndPoint, RemoteEndPoint, false);
            }
            finally
            {
                ServerStart = false;
            }
        }
        private Task ListenAsync()
        {
            _acceptTokenSource = _acceptTokenSource.NewTokenSource();
            var localEP = new IPEndPoint(IPAddress.Parse(Options.IP), Options.Port);
            _server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _server.Bind(localEP);
            _server.Listen(10);
            ConnectionState = ConnectionState.NotConnected;
            return Task.Run(async () =>
            {
                try
                {
                    while (!_acceptTokenSource.IsCancellationRequested)
                    {
                        try
                        {
                            var client = _server.Accept();
                            ConnectionState = ConnectionState.Connected;
                            LocalEndPoint = client.LocalEndPoint;
                            RemoteEndPoint = client.RemoteEndPoint;
                            SessionConnectionChanged?.Invoke(LocalEndPoint, RemoteEndPoint, true);
                            _callerSocket = client;
                            await LoopReceiveAsync();
                            SessionConnectionChanged?.Invoke(LocalEndPoint, RemoteEndPoint, false);
                        }
                        catch (Exception ex)
                        {
                            InternalException?.Invoke("Accept error", ex);
                        }
                        await Task.Delay(Options.T5, _acceptTokenSource.Token);
                    }
                }
                catch (Exception ex)
                {
                    InternalException?.Invoke("Listen error", ex);
                }
            });
        }
        private int Send(Socket socket, byte[] buffer)
        {
            int len = socket.Send(buffer);
            RawMessageChanged?.Invoke(buffer, RawType.Send);
            return len;
        }
        private Task LoopSendLinkTestAsync()
        {
            _loopLinkTestTokenSource = _loopLinkTestTokenSource.NewTokenSource();
            return Task.Run(async () =>
            {
                while (!_loopLinkTestTokenSource.IsCancellationRequested)
                {
                    try
                    {
                        await Task.Delay(Options.LinkTestInterval, _loopLinkTestTokenSource.Token);
                        if (_loopLinkTestTokenSource.IsCancellationRequested) return;

                        if (_callerSocket == null || !_callerSocket.Connected)
                        {
                            await Task.Delay(1, _loopLinkTestTokenSource.Token);
                            if (_loopLinkTestTokenSource.IsCancellationRequested) return;
                            continue;
                        }

                        SendLinktestReq();
                    }
                    catch (Exception ex)
                    {
                        if (_loopLinkTestTokenSource.IsCancellationRequested) return;
                        InternalException?.Invoke($"SendLinktest error, Local:{LocalEndPoint}, Remote:{RemoteEndPoint}", ex);
                    }
                }
            });
        }
        private async Task LoopReceiveAsync()
        {
            _loopReceiveTokenSource = _loopReceiveTokenSource.NewTokenSource();
            var readDataBuffer = new byte[1024 * 1024];
            while (!_loopReceiveTokenSource.IsCancellationRequested)
            {
                if (_loopReceiveTokenSource.IsCancellationRequested ||
                    _callerSocket == null ||
                    !_callerSocket.Connected) return;

                try
                {
                    _callerSocket.ReceiveTimeout = Timeout.Infinite;
                    int count = _callerSocket.Receive(readDataBuffer, 0, 4, SocketFlags.None);
                    if (count == 0)
                    {
                        await Task.Delay(1, _loopReceiveTokenSource.Token);
                        continue;
                    }

                    int payloadLength = GetPayloadLength(readDataBuffer);
                    _callerSocket.ReceiveTimeout = Options.T8;
                    count = _callerSocket.Receive(readDataBuffer, 4, payloadLength, SocketFlags.None);
                    if (count != payloadLength)
                        throw new Exception("Receive timeout T8");

                    int totalLength = payloadLength + 4;
                    var readData = readDataBuffer.Slice(0, totalLength);
                    RawMessageChanged?.Invoke(readData, RawType.Receive);
                    await AddOrUpdateDataContextAsync(readData);
                }
                catch (Exception ex)
                {
                    if (_loopReceiveTokenSource.IsCancellationRequested) return;

                    if (_callerSocket == null || !_callerSocket.Connected)
                    {
                        _loopReceiveTokenSource.Destroy();
                        InternalException?.Invoke($"Socket disconnected, Local:{LocalEndPoint}, Remote:{RemoteEndPoint}", ex);
                        return;
                    }
                    InternalException?.Invoke($"Loop receive error, Local:{LocalEndPoint}, Remote:{RemoteEndPoint}", ex);
                }

                static int GetPayloadLength(byte[] buffer)
                    => (buffer[0] << 24) + (buffer[1] << 16) + (buffer[2] << 8) + buffer[3];
            }
        }
        private async Task AddOrUpdateDataContextAsync(byte[] readData)
        {
            var hsmsMessage = new HsmsMessage(readData);
            try
            {
                byte s = hsmsMessage.Header.Stream;
                byte f = hsmsMessage.Header.Function;
                var sType = hsmsMessage.Header.SType;
                if (s == 0 && f == 0)
                {
                    await AddOrUpdateControlDataContextAsync(hsmsMessage, sType);
                }
                else
                {
                    await AddOrUpdateDataMessageDataContextAsync(hsmsMessage, f);
                }
            }
            finally
            {
                HsmsMessageChanged?.Invoke(hsmsMessage);
            }
        }
        private async Task AddOrUpdateControlDataContextAsync(HsmsMessage hsmsMessage, SType sType)
        {
            if (sType != SType.DataMessage)
            {
                if ((int)sType % 2 > 0)
                {
                    //function为奇数，为请求消息，被动接收的数据 EAP => EQP
                    await AddOrUpdateSecondaryDataContextAsync(hsmsMessage, true);
                }
                else
                {
                    //function为偶数，为回复消息，被动接收的数据
                    await AddOrUpdatePrimaryDataContextAsync(hsmsMessage, false);
                }
            }
        }
        private async Task AddOrUpdateDataMessageDataContextAsync(HsmsMessage hsmsMessage, byte f)
        {
            if (f % 2 > 0)
            {
                //function为奇数，为请求消息，被动接收的数据 EAP => EQP
                await AddOrUpdateSecondaryDataContextAsync(hsmsMessage, true);
            }
            else
            {
                //function为偶数，为回复消息，被动接收的数据
                await AddOrUpdatePrimaryDataContextAsync(hsmsMessage, false);
            }
        }
        private async Task AddOrUpdatePrimaryDataContextAsync(HsmsMessage hsmsMessage, bool isRequest)
        {
            await _primaryContextSemaphoreSlim.WaitAsync();
            try
            {
                AddOrUpdateDataContext(_primaryContext, hsmsMessage, isRequest);
            }
            finally
            {
                _primaryContextSemaphoreSlim.Release();
            }
        }
        private async Task AddOrUpdateSecondaryDataContextAsync(HsmsMessage hsmsMessage, bool isRequest)
        {
            await _secondaryContextSemaphoreSlim.WaitAsync();
            try
            {
                AddOrUpdateDataContext(_secondaryContext, hsmsMessage, isRequest);
            }
            finally
            {
                _secondaryContextSemaphoreSlim.Release();
            }
        }
        private void AddOrUpdateDataContext(Dictionary<int, HsmsDataContext> context, HsmsMessage hsmsMessage, bool isRequest)
        {
            int sb = hsmsMessage.Header.SystemBytes;
            if (context.TryGetValue(sb, out var dc))
            {
                dc.Caller = _callerSocket;
                if (isRequest)
                {
                    dc.Request = new Transport()
                    {
                        RawData = hsmsMessage.RawData,
                        Message = hsmsMessage,
                    };
                }
                else
                {
                    dc.Response = new Transport()
                    {
                        RawData = hsmsMessage.RawData,
                        Message = hsmsMessage,
                    };
                }
            }
            else
            {
                dc = new HsmsDataContext()
                {
                    Caller = _callerSocket,
                };
                if (isRequest)
                {
                    dc.Direction = TransportDirection.H2E;
                    dc.Request = new Transport()
                    {
                        RawData = hsmsMessage.RawData,
                        Message = hsmsMessage,
                    };
                    context.Add(sb, dc);
                }
                else
                {
                    //未进行请求前，回复数据无效
                    //dc.Direction = TransportDirection.EQP2EAP;
                    //dc.Response = new Transport()
                    //{
                    //    RawData = readData,
                    //    Message = readData.ToHsmsMessage(),
                    //};
                }
            }
        }
        private Task LoopProcessSecondaryContextsAsync()
        {
            _loopProcessSecondaryContextsTokenSource = _loopProcessSecondaryContextsTokenSource.NewTokenSource();
            var deleteList = new List<int>();
            return Task.Run(async () =>
            {
                while (!_loopProcessSecondaryContextsTokenSource.IsCancellationRequested)
                {
                    if (_loopProcessSecondaryContextsTokenSource.IsCancellationRequested) return;

                    await _secondaryContextSemaphoreSlim.WaitAsync();
                    try
                    {
                        if (_secondaryContext.Count == 0)
                        {
                            await Task.Delay(1, _loopProcessSecondaryContextsTokenSource.Token);
                            continue;
                        }

                        deleteList.Clear();
                        foreach (var item in _secondaryContext)
                        {
                            ExecuteSecondaryDataContext(item.Value);
                            if (item.Value.Handled)
                            {
                                deleteList.Add(item.Key);
                            }
                        }

                        if (deleteList.Count > 0)
                        {

                            foreach (var item in deleteList)
                            {
                                _secondaryContext.Remove(item);
                            }
                        }

                        ClearExpiredDataContexts(_secondaryContext);
                    }
                    catch (Exception ex)
                    {
                        if (_loopProcessSecondaryContextsTokenSource.IsCancellationRequested) return;

                        InternalException?.Invoke($"Process secondary context error, Local:{LocalEndPoint}, Remote:{RemoteEndPoint}", ex);
                    }
                    finally
                    {
                        _secondaryContextSemaphoreSlim.Release();
                    }
                }
            });
        }
        private void ExecuteSecondaryDataContext(HsmsDataContext dataContext)
        {
            try
            {
                if (!dataContext.Handled && dataContext.Request?.Message is HsmsMessage rspMsg)
                {
                    ushort sessionId = rspMsg.Header.SessionId;
                    byte s = rspMsg.Header.Stream;
                    byte f = rspMsg.Header.Function;
                    var pType = rspMsg.Header.PType;
                    int systemBytes = rspMsg.Header.SystemBytes;
                    var sType = rspMsg.Header.SType;
                    switch (sType)
                    {
                        case SType.SelectReq:
                            {
                                var header = new HsmsHeader(sessionId, s, f, SType.SelectRsp, systemBytes);
                                var hsmsMsg = new HsmsMessage(header);
                                var rawData = HsmsMessage.ConverterToBytes(hsmsMsg);
                                dataContext.Response = new Transport()
                                {
                                    Message = hsmsMsg,
                                    RawData = rawData
                                };
                                if (dataContext.Caller is null)
                                    throw new NullReferenceException("Caller socket is null");

                                ConnectionState = ConnectionState.NotSelected;
                                Send(dataContext.Caller, rawData);
                                ConnectionState = ConnectionState.Selected;
                                HsmsMessageChanged?.Invoke(hsmsMsg);
                                break;
                            }
                        case SType.DeselectReq:
                            {
                                var header = new HsmsHeader(sessionId, s, f, SType.DeselectRsp, systemBytes);
                                var hsmsMsg = new HsmsMessage(header);
                                var rawData = HsmsMessage.ConverterToBytes(hsmsMsg);
                                dataContext.Response = new Transport()
                                {
                                    Message = hsmsMsg,
                                    RawData = rawData
                                };
                                if (dataContext.Caller is null)
                                    throw new NullReferenceException("Caller socket is null");

                                Send(dataContext.Caller, rawData);
                                ConnectionState = ConnectionState.NotSelected;
                                HsmsMessageChanged?.Invoke(hsmsMsg);
                                break;
                            }
                        case SType.LinktestReq:
                            {
                                var header = new HsmsHeader(sessionId, s, f, SType.LinktestRsp, systemBytes);
                                var hsmsMsg = new HsmsMessage(header);
                                var rawData = HsmsMessage.ConverterToBytes(hsmsMsg);
                                dataContext.Response = new Transport()
                                {
                                    Message = hsmsMsg,
                                    RawData = rawData
                                };
                                if (dataContext.Caller is null)
                                    throw new NullReferenceException("Caller socket is null");

                                Send(dataContext.Caller, rawData);
                                HsmsMessageChanged?.Invoke(hsmsMsg);
                                break;
                            }
                        case SType.SeparateReq:
                            {
                                ConnectionState = ConnectionState.NotSelected;
                                _callerSocket.SafeClose();
                                _callerSocket = null;
                                ConnectionState = ConnectionState.NotConnected;
                                break;
                            }
                        case SType.DataMessage:
                            {
                                var res = SubscribeRemoteCaller?.Invoke(rspMsg);
                                if (res != null)
                                {
                                    var buffer = HsmsMessage.ConverterToBytes(res);
                                    dataContext.Response = new Transport()
                                    {
                                        Message = res,
                                        RawData = buffer
                                    };
                                    if (dataContext.Caller is null)
                                        throw new NullReferenceException("Caller socket is null");

                                    Send(dataContext.Caller, buffer);
                                    HsmsMessageChanged?.Invoke(res);
                                }

                                break;
                            }
                        case SType.DeselectRsp:
                            {


                                break;
                            }
                        case SType.SelectRsp:
                        case SType.LinktestRsp:
                        case SType.RejectRsp:
                            throw new NotImplementedException($"Unsupported SType `{sType}`");
                    }
                }
            }
            catch (Exception ex)
            {
                InternalException?.Invoke($"Execute secondary context error, DataContext:{dataContext}", ex);
            }
            finally
            {
                dataContext.Handled = true;
                dataContext.HandleTimestamp = DateTime.Now;
                HsmsDataContextChanged?.Invoke(dataContext);
            }
        }
        private void CreateDataMessageHsmsDataContext(byte stream, byte function, HsmsBody? body, out int sb, out byte[] data)
        {
            sb = SystemBytesHelper.RandomSystemBytes();
            var header = new HsmsHeader(Options.SessionId, stream, function, SType.DataMessage, sb, true);
            var msg = new HsmsMessage(header, body);
            data = HsmsMessage.ConverterToBytes(msg);
            var dc = new HsmsDataContext()
            {
                Direction = TransportDirection.E2H,
                Caller = _callerSocket,
                Request = new Transport()
                {
                    RawData = data,
                    Message = msg,
                }
            };
            _primaryContextSemaphoreSlim.Wait();
            try
            {
                _primaryContext.Add(sb, dc);
            }
            finally
            {
                _primaryContextSemaphoreSlim.Release();
            }
        }
        private void CreateControlHsmsDataContext(SType type, out int sb, out byte[] data)
        {
            sb = SystemBytesHelper.RandomSystemBytes();
            var header = new HsmsHeader(DefaultSessionId, 0, 0, type, sb, false);
            var requestMsg = new HsmsMessage(header);
            data = HsmsMessage.ConverterToBytes(requestMsg);
            var dc = new HsmsDataContext()
            {
                Direction = TransportDirection.E2H,
                Caller = _callerSocket,
                Request = new Transport()
                {
                    RawData = data,
                    Message = requestMsg,
                }
            };
            _primaryContextSemaphoreSlim.Wait();
            try
            {
                _primaryContext.Add(sb, dc);
            }
            finally
            {
                _primaryContextSemaphoreSlim.Release();
            }
        }
        private HsmsMessage ReplyHsmsMessage(int sb)
        {
            var sw = Stopwatch.StartNew();
            while (true)
            {
                _primaryContextSemaphoreSlim.Wait();
                try
                {
                    if (_primaryContext.TryGetValue(sb, out HsmsDataContext dataContext) &&
                        dataContext.Response is ITransport tran &&
                        tran.Message is HsmsMessage rsp)
                    {
                        dataContext.Handled = true;
                        dataContext.HandleTimestamp = DateTime.Now;
                        HsmsDataContextChanged?.Invoke(dataContext);
                        _primaryContext.Remove(sb);
                        ClearExpiredDataContexts(_primaryContext);
                        return rsp;
                    }
                    else
                    {
                        Thread.Sleep(1);
                    }
                    if (sw.ElapsedMilliseconds > Options.T3)
                        throw new ReplyTimeoutException(dataContext);
                }
                finally
                {
                    _primaryContextSemaphoreSlim.Release();
                }
            }
        }
        private void ClearExpiredDataContexts(Dictionary<int, HsmsDataContext> contexts)
        {
            if (contexts.Count > 0)
            {
                var list = new List<int>();
                foreach (var item in contexts)
                {
                    if ((item.Value.Timestamp - DateTime.Now).TotalHours > 1)
                    {
                        list.Add(item.Key);
                    }
                }
                foreach (var item in list)
                {
                    contexts.Remove(item);
                }
            }
        }
        public HsmsMessage SendDataMessage(byte stream, byte function, HsmsBody body)
        {
            CreateDataMessageHsmsDataContext(stream, function, body, out int sb, out byte[] data);
            if (_callerSocket is null)
                throw new NullReferenceException("Caller socket is null");

            Send(_callerSocket, data);
            return ReplyHsmsMessage(sb);
        }
        public Task<HsmsMessage> SendDataMessageAsync(byte stream, byte function, HsmsBody body, CancellationToken cancellationToken = default)
        {
            var tcs = new TaskCompletionSource<HsmsMessage>(TaskCreationOptions.RunContinuationsAsynchronously);
            if (cancellationToken.CanBeCanceled)
            {
                cancellationToken.Register(() =>
                {
                    tcs.TrySetCanceled(cancellationToken);
                });
            }
            Task.Run(() =>
            {
                try
                {
                    var result = SendDataMessage(stream, function, body);
                    tcs.SetResult(result);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });
            return tcs.Task;
        }
        public HsmsMessage SendDataMessage(byte stream, byte function, string sml)
        {
            var body = new HsmsBody(sml);
            return SendDataMessage(stream, function, body);
        }
        public Task<HsmsMessage> SendDataMessageAsync(byte stream, byte function, string sml, CancellationToken cancellationToken = default)
        {
            var body = new HsmsBody(sml);
            return SendDataMessageAsync(stream, function, body, cancellationToken);
        }
        private HsmsMessage SendControlMessage(SType type)
        {
            if (type == SType.DataMessage)
                throw new Exception("Send ask unsupported `DataMessage`");

            CreateControlHsmsDataContext(type, out int sb, out byte[] data);
            if (_callerSocket is null)
                throw new NullReferenceException("Caller socket is null");

            Send(_callerSocket, data);
            return ReplyHsmsMessage(sb);
        }
        public HsmsMessage SendSelectReq()
        {
            return SendControlMessage(SType.SelectReq);
        }
        public HsmsMessage SendDeselectReq()
        {
            return SendControlMessage(SType.DeselectReq);
        }
        public HsmsMessage SendLinktestReq()
        {
            return SendControlMessage(SType.LinktestReq);
        }
        public HsmsMessage SendRejectRsp()
        {
            return SendControlMessage(SType.RejectRsp);
        }
        public void SendSeparateReq()
        {
            CreateControlHsmsDataContext(SType.SeparateReq, out int sb, out byte[] data);
            if (_callerSocket is null)
                throw new NullReferenceException("Caller socket is null");

            Send(_callerSocket, data);

            _primaryContextSemaphoreSlim.Wait();
            try
            {
                if (_primaryContext.TryGetValue(sb, out var dc))
                {
                    dc.Handled = true;
                    dc.HandleTimestamp = DateTime.Now;
                }
            }
            finally
            {
                _primaryContextSemaphoreSlim.Release();
            }
        }
        private Task<HsmsMessage> SendControlMessageAsync(SType type, CancellationToken cancellationToken = default)
        {
            var tcs = new TaskCompletionSource<HsmsMessage>(TaskCreationOptions.RunContinuationsAsynchronously);
            if (cancellationToken.CanBeCanceled)
            {
                cancellationToken.Register(() =>
                {
                    tcs.TrySetCanceled(cancellationToken);
                });
            }
            Task.Run(() =>
            {
                try
                {
                    var result = SendControlMessage(type);
                    tcs.SetResult(result);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });
            return tcs.Task;
        }
        public Task<HsmsMessage> SendSelectReqAsync()
        {
            return SendControlMessageAsync(SType.SelectReq);
        }
        public Task<HsmsMessage> SendDeselectReqAsync()
        {
            return SendControlMessageAsync(SType.DeselectReq);
        }
        public Task<HsmsMessage> SendLinktestReqAsync()
        {
            return SendControlMessageAsync(SType.LinktestReq);
        }
        public Task<HsmsMessage> SendRejectRspAsync()
        {
            return SendControlMessageAsync(SType.RejectRsp);
        }
        public Task SendSeparateReqAsync(CancellationToken cancellationToken = default)
        {
            var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            if (cancellationToken.CanBeCanceled)
            {
                cancellationToken.Register(() =>
                {
                    tcs.TrySetCanceled(cancellationToken);
                });
            }
            Task.Run(() =>
            {
                try
                {
                    CreateControlHsmsDataContext(SType.SeparateReq, out int sb, out byte[] data);
                    if (_callerSocket is null)
                        throw new NullReferenceException("Caller socket is null");

                    Send(_callerSocket, data);
                    tcs.SetResult(true);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });
            return tcs.Task;
        }
    }
}