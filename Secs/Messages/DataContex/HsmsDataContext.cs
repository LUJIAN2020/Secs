using Secs.Enums;
using Secs.Extensions;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Secs.Messages
{
    public class HsmsDataContext
    {
        public Socket? Caller { get; internal set; }
        public EndPoint? Local => Caller.GetLocalEndPoint();
        public EndPoint? Remote => Caller.GetRemoteEndPoint();
        public TransportDirection Direction { get; internal set; }
        public ITransport? Request { get; internal set; }
        public ITransport? Response { get; internal set; }
        public bool Handled { get; internal set; } = false;
        public DateTime HandleTimestamp { get; internal set; }
        public DateTime Timestamp { get; internal set; } = new DateTime();
        public override string ToString()
        {
            string dir = Direction == TransportDirection.E2H ? "Equipment => Host" : "Host => Equipment";
            var sb = new StringBuilder();
            sb.AppendLine($"[TransportDirection] - {dir}")
                .AppendLine($"[LocalEndPoint] - {Local}")
                .AppendLine($"[RemoteEndPoint] - {Remote}")
                .AppendLine($"[HandleTimestamp] - {HandleTimestamp:yyyy-MM-dd HH:mm:ss.fff}")
                .AppendLine($"[Handled] - {Handled}");
            ITransport? req, rsp;
            if (Direction == TransportDirection.E2H)
            {
                req = Request;
                rsp = Response;
            }
            else
            {
                req = Response;
                rsp = Request;
            }
            sb.AppendLine($"[TX] - {req?.RawData.ToHexString()}")
                .AppendLine($"[RX] - {rsp?.RawData.ToHexString()}")
                .Append($"[SEND] - {req?.Timestamp:yyyy-MM-dd HH:mm:ss.fff}, ")
                .Append($"S{req?.Message?.Header.Stream & 0x0F}F{req?.Message?.Header.Function}, ")
                .Append($"SType={req?.Message?.Header.SType}, ")
                .Append($"SB={req?.Message?.Header.SystemBytes}, ")
                .Append($"Length={req?.Message?.RawData.Length}\r\n");
            if (req != null && req.Message != null)
            {
                sb.AppendLine(HsmsMessage.ConverterToSml(req.Message, true));
            }

            sb.Append($"[RECV] - {rsp?.Timestamp:yyyy-MM-dd HH:mm:ss.fff}, ")
                .Append($"S{rsp?.Message?.Header.Stream & 0x0F}F{rsp?.Message?.Header.Function}, ")
                .Append($"SType={rsp?.Message?.Header.SType}, ")
                .Append($"SB={rsp?.Message?.Header.SystemBytes}, ")
                .Append($"Length={rsp?.Message?.RawData.Length}\r\n");
            if (rsp != null && rsp.Message != null)
            {
                sb.AppendLine(HsmsMessage.ConverterToSml(rsp.Message, true));
            }
            return sb.ToString().Trim();
        }
        public EndPoint? GetLocalEndPoint(Socket? socket)
        {
            try
            {
                if (socket != null)
                    return socket.LocalEndPoint;
            }
            catch { }
            return default;
        }
        public EndPoint? GetRemoteEndPoint(Socket? socket)
        {
            try
            {
                if (socket != null)
                    return socket.RemoteEndPoint;
            }
            catch { }
            return default;

        }
    }
}
