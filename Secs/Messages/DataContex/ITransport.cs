using System;

namespace Secs.Messages
{
    public interface ITransport
    {
        HsmsMessage? Message { get; set; }
        byte[] RawData { get; set; }
        DateTime Timestamp { get; }
        string Sml { get; }
    }
}
