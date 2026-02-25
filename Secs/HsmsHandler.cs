using Secs.Enums;
using Secs.Messages;
using System;
using System.Net;

namespace Secs
{
    /// <summary>
    /// Connection session changed
    /// </summary>
    /// <param name="localEndPoint">local endpoint</param>
    /// <param name="remoteEndPoint">remote endpoint</param>
    /// <param name="isConnected">true=connected,false=disconnect</param>
    public delegate void SessionConnectionChanged(EndPoint? localEndPoint, EndPoint? remoteEndPoint, bool isConnected);
    /// <summary>
    /// Internal exception
    /// </summary>
    /// <param name="message">exception message</param>
    /// <param name="exception">exception instance</param>
    public delegate void InternalException(string message, Exception exception);
    /// <summary>
    /// Record send or receive raw message
    /// </summary>
    /// <param name="buffer">raw data</param>
    /// <param name="rawType">send or receive</param>
    public delegate void RawMessageChanged(byte[] buffer, RawType rawType);
    /// <summary>
    /// Hsms data context changed
    /// </summary>
    /// <param name="hsmsDataContext"></param>
    public delegate void HsmsDataContextChanged(HsmsDataContext hsmsDataContext);
    /// <summary>
    /// Record send or receive hsms message
    /// </summary>
    /// <param name="hsmsMessage"></param>
    public delegate void HsmsMessageChanged(HsmsMessage hsmsMessage);
    /// <summary>
    /// connection state changed
    /// </summary>
    /// <param name="connectionState"></param>
    public delegate void ConnectionStateChanged(ConnectionState connectionState);
    /// <summary>
    /// Subscribe Remote Invocation Delegate: 
    /// A delegate used to passively receive messages from a remote entity. 
    /// It serves as the primary delegate for handling business logic.
    /// </summary>
    /// <param name="hsmsMessage"></param>
    /// <returns>A reply that is not mandatory can be left blank</returns>
    public delegate HsmsMessage? SubscribeRemoteCaller(HsmsMessage hsmsMessage);
}