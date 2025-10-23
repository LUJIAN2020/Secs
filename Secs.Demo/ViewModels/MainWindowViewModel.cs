using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using Secs.Demo.Commons;
using Secs.Demo.Models;
using Secs.Demo.Services;
using Secs.Demo.Views;
using Secs.Enums;
using Secs.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Secs.Demo.ViewModels
{
    public partial class MainWindowViewModel(INotificationService notificationService, ILogService log) : ObservableObject
    {
        private HsmsServer? server;
        private static SmlSendWindow? smlSendWindow;
        private SmlItem[] smlItems = Array.Empty<SmlItem>();
        private string smlFileName = string.Empty;
        private readonly DispatcherTimer dispatcherTimer = new();
        [ObservableProperty] private string? _localEP;
        [ObservableProperty] private string? _remoteEP;
        [ObservableProperty] private bool _isConnect;
        [ObservableProperty] private bool _isStart;
        [ObservableProperty] private string? _sendData;
        [ObservableProperty] private string? _receiveData;
        [ObservableProperty] private string? _exceptionMessage;
        [ObservableProperty] private string? _hsmsDataContextText;
        [ObservableProperty] private string? _hsmsMessageText;
        [ObservableProperty] private string? _connectionStateText;
        [ObservableProperty] private string? _connectionModeText;
        [ObservableProperty] private int _primaryContextCount;
        [ObservableProperty] private int _secondaryContextCount;
        public void StartCommand()
        {
            try
            {
                var config = ConfigHelper.LoadConfig();
                string ip = config.LocalIP;
                ushort port = config.LocalPort;
                ushort deviceId = config.Device;
                bool isActive = config.ActiveIsChecked;
                var options = new HsmsOptions()
                {
                    SessionId = deviceId,
                    IP = ip,
                    Port = port,
                    ConnectionMode = isActive ? ConnectionMode.Active : ConnectionMode.Passive,
                    LinkTest = false,
                };
                ConnectionModeText = options.ConnectionMode.ToString();
                server = new HsmsServer(options)
                {
                    InternalException = OnInternalException,
                    RawMessageChanged = OnRawMessageChanged,
                    HsmsDataContextChanged = OnHsmsDataContextChanged,
                    HsmsMessageChanged = OnHsmsMessageChanged,
                    SessionConnectionChanged = OnSessionConnectionChanged,
                    SubscribeRemoteCaller = OnSubscribeRemoteCaller,
                    ConnectionStateChanged = OnConnectionStateChanged
                };
                server.Start();
                IsStart = true;
                string msg = $"Server start, IP:{ip}, Port:{port}, DeviceId:{deviceId}";
                log.Info(msg);
                notificationService.ShowInfo(msg, nameof(MainWindow));
                LocalEP = server.LocalEndPoint?.ToString();
                RemoteEP = server.RemoteEndPoint?.ToString();
                dispatcherTimer.Tick += DispatcherTimer_Tick;
                dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
                dispatcherTimer.Start();
            }
            catch (Exception ex)
            {
                notificationService.ShowError(ex.Message, nameof(MainWindow));
            }
        }
        private void DispatcherTimer_Tick(object? sender, EventArgs e)
        {
            PrimaryContextCount = server?.PrimaryContextCount ?? 0;
            SecondaryContextCount = server?.SecondaryContextCount ?? 0;
        }
        public void StopCommand()
        {
            server?.Stop();
            IsStart = false;
            log.Info("Server stop");
            notificationService.ShowInfo("Server stop", nameof(MainWindow));
            dispatcherTimer.Tick -= DispatcherTimer_Tick;
            dispatcherTimer.Stop();
        }
        public async Task LoadSmlCommand()
        {
            var topLevel = TopLevel.GetTopLevel(App.MainWindow);
            if (topLevel != null)
            {
                var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                {
                    Title = "Open SML File",
                    AllowMultiple = false,
                    FileTypeFilter =
                    [
                        new ("SML File")
                        {
                            Patterns = ["*.sml", "*.SML"]
                        }
                     ]
                });
                if (files.Count >= 1)
                {
                    smlFileName = files[0].Path.LocalPath;
                    await using var stream = await files[0].OpenReadAsync();
                    var cmds = new List<string>();
                    using (var sr = new StreamReader(stream))
                    {
                        var fileContent = await sr.ReadToEndAsync();
                        smlItems = SmlFileHelper.LoadToSmlItemsFromText(fileContent);
                        ShowSmlSendWindow(smlItems, smlFileName, server);
                    }
                }
            }
        }
        public void SendSmlCommand()
        {
            if (!string.IsNullOrWhiteSpace(smlFileName))
            {
                smlItems = SmlFileHelper.LoadToSmlItems(smlFileName);
                ShowSmlSendWindow(smlItems, smlFileName, server);
            }
            else
            {
                notificationService.ShowInfo("Please load sml file", nameof(MainWindow));
            }
        }
        public async Task AddSmlCommand()
        {
            var topLevel = TopLevel.GetTopLevel(App.MainWindow);
            if (topLevel != null)
            {
                var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
                {
                    Title = "Save SML File",
                    DefaultExtension = "sml",
                    FileTypeChoices =
                    [
                        new ("SML File")
                        {
                            Patterns = ["*.sml", "*.SML"]
                        }
                     ]
                });
                if (file is not null)
                {
                    await using (var stream = await file.OpenWriteAsync())
                    {
                        using (var streamWriter = new StreamWriter(stream))
                        {
                            await streamWriter.WriteLineAsync("");
                        }
                    }
                    smlFileName = file.Path.LocalPath;
                    smlItems = SmlFileHelper.LoadToSmlItems(smlFileName);
                    ShowSmlSendWindow(smlItems, smlFileName, server);
                }
            }
        }
        private void ShowSmlSendWindow(SmlItem[] smlItems, string smlFileName, HsmsServer? server)
        {
            if (smlSendWindow != null)
            {
                if (smlSendWindow.WindowState == WindowState.Minimized)
                {
                    smlSendWindow.WindowState = WindowState.Normal;
                }
                smlSendWindow.Activate();
            }
            else
            {
                var vm = App.GetService<SmlSendWindowViewModel>();
                if (vm != null)
                {
                    vm.SmlItems = new(smlItems);
                    vm.FilePath = smlFileName;
                    vm.SaveHandler = () =>
                    {
                        smlItems = SmlFileHelper.LoadToSmlItems(smlFileName);
                    };
                    vm.SendHandler = (s, f, body) =>
                    {
                        if (server != null)
                        {
                            server.SendDataMessage(s, f, body);
                            notificationService.ShowSuccess("Send success", nameof(SmlSendWindow));
                        }
                        else
                        {
                            notificationService.ShowInfo("Server not start", nameof(SmlSendWindow));
                        }
                    };
                    smlSendWindow = new SmlSendWindow() { DataContext = vm, ShowInTaskbar = true };
                    smlSendWindow.Closed += (_, _) => smlSendWindow = null;
                    smlSendWindow.Show();
                }
            }
        }
        private void OnInternalException(string? msg, Exception ex)
        {
            log.Error(ex, msg);
            notificationService.ShowError(msg + ex.Message, nameof(MainWindow));
        }
        private void OnRawMessageChanged(byte[] data, RawType type)
        {
            string hex = string.Join(" ", data.Select(c => c.ToString("X2")));
            if (type == RawType.Send)
            {
                SendData = hex;
                log.Info($"TX:{hex}");
            }
            else
            {
                ReceiveData = hex;
                log.Info($"RX:{hex}");
            }
        }
        private void OnHsmsDataContextChanged(HsmsDataContext context)
        {
            HsmsDataContextText = context.ToString();
            log.Info(HsmsDataContextText);
        }
        private void OnHsmsMessageChanged(HsmsMessage msg)
        {
            HsmsMessageText = HsmsMessage.ConverterToSml(msg, true);
            log.Info(HsmsMessageText);
        }
        private void OnConnectionStateChanged(Secs.Enums.ConnectionState state)
        {
            ConnectionStateText = state.ToString();
            string msg = $"ConnectionStateChanged:{state}";
            log.Info(msg);
            notificationService.ShowInfo(msg, nameof(MainWindow));
        }
        private void OnSessionConnectionChanged(EndPoint? local, EndPoint? remote, bool isConnect)
        {
            LocalEP = local?.ToString();
            RemoteEP = remote?.ToString();
            IsConnect = isConnect;
            string msg = $"SessionConnectionChanged,ConnectState:{IsConnect}, Local:{LocalEP}, Remote:{RemoteEP}";
            log.Info(msg);
            notificationService.ShowInfo(msg, nameof(MainWindow));
        }
        private HsmsMessage OnSubscribeRemoteCaller(HsmsMessage req)
        {
            //service code


            var rspHeader = HsmsHeader.CreateDefaultReplyHsmsHeader(req.Header);
            var body = new L
            (
                "Response",
                new HsmsBody[]
                {
                    new U1(0, "ErrorCode"),
                    new A("Success", "Message"),
                }
            );
            return new HsmsMessage(rspHeader, body);
        }
        public void ConfigCommand()
        {
            var config = App.GetService<ConfigWindow>();
            config?.ShowDialog(App.MainWindow);
        }
    }
}
