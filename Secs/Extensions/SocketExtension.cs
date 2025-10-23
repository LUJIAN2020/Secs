using System;
using System.Net;
using System.Net.Sockets;

namespace Secs.Extensions
{
    internal static class SocketExtension
    {
        public static void SafeClose(this Socket? socket)
        {
            try
            {
                if (socket is not null && socket.Connected)
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Disconnect(false);
                }
            }
            catch { }
            finally
            {
                try
                {
                    socket?.Close();
                    socket?.Dispose();
                    socket = null;
                }
                catch { }
            }
        }
        public static void Connect(this Socket socket, IPEndPoint endPoint, int connectTimeout = 1000)
        {
            if (socket is null)
                throw new ArgumentNullException(nameof(socket));

            if (endPoint is null)
                throw new ArgumentNullException(nameof(endPoint));

            if (connectTimeout > 0)
            {
                IAsyncResult result = socket.BeginConnect(endPoint, null, null);
                result.AsyncWaitHandle.WaitOne(connectTimeout, true);

                if (socket.Connected)
                {
                    socket.EndConnect(result);
                }
                else
                {
                    socket.SafeClose();
                    throw new Exception($"Failed to connect server {endPoint.Address}:{endPoint.Port}");
                }
            }
            else
            {
                socket.Connect(endPoint);
            }
        }
        public static EndPoint? GetLocalEndPoint(this Socket? socket)
        {
            try
            {
                if (socket != null)
                    return socket.LocalEndPoint;
            }
            catch { }
            return default;
        }
        public static EndPoint? GetRemoteEndPoint(this Socket? socket)
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
