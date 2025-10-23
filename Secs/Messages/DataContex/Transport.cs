using System;

namespace Secs.Messages
{
    public class Transport : ITransport
    {
        public HsmsMessage? Message { get; set; }
        public byte[] RawData { get; set; } = Array.Empty<byte>();
        public DateTime Timestamp { get; } = DateTime.Now;
        public string Sml => Message != null ? HsmsMessage.ConverterToSml(Message) : string.Empty;
    }
}