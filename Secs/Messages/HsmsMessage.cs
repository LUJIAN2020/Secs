using Secs.Extensions;
using System;
using System.Linq;
using System.Text;

namespace Secs.Messages
{
    /// <summary>
    /// Message structure
    /// </summary>
    /// <remarks>
    /// 4-byte length + 10-byte message header + data
    /// </remarks>
    public class HsmsMessage
    {
        public HsmsMessage(HsmsHeader header, HsmsBody? body = default)
        {
            Header = header;
            Body = body;

            var headerBytes = HsmsHeader.ConverterToBytes(header);
            var bodyBytes = body != null
                ? HsmsBody.ConverterToBytes(body)
                : Array.Empty<byte>();

            Length = headerBytes.Length + bodyBytes.Length;
            var lens = BitConverter.GetBytes(Length).Reverse().ToArray();
            RawData = lens.Combine(headerBytes, bodyBytes);
            Timestamp = DateTime.Now;
        }
        public HsmsMessage(byte[] buffer)
        {
            if (buffer.Length < 14)
                throw new ArgumentOutOfRangeException("buffer", buffer.Length, "The minimum data length is 14 bytes");

            int len = (buffer[0] << 24) + (buffer[1] << 16) + (buffer[2] << 8) + buffer[3];
            if (len != buffer.Length - 4)
                throw new ArgumentOutOfRangeException("buffer", buffer.Length, $"Body bytes length does not match actual bytes length. Body bytes length: {len}, actual bytes length: {buffer.Length - 4}");

            Header = new HsmsHeader(buffer.Slice(4, 10));
            if (buffer.Length > 14)
            {
                Body = new HsmsBody(buffer.Slice(14));
            }
            Length = buffer.Length;
            RawData = buffer;
            Timestamp = DateTime.Now;
        }

        /// <summary>
        /// Data length
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Header
        /// </summary>
        public HsmsHeader Header { get; }

        /// <summary>
        /// Payload data
        /// </summary>
        public HsmsBody? Body { get; }

        /// <summary>
        /// Parsed raw byte data
        /// </summary>
        public byte[] RawData { get; internal set; }

        /// <summary>
        /// Timestamp
        /// </summary>
        public DateTime Timestamp { get; }

        /// <summary>
        /// Message description
        /// </summary>
        public string? Description { get; set; }

        public static byte[] ConverterToBytes(HsmsMessage message)
        {
            var headerBytes = HsmsHeader.ConverterToBytes(message.Header);
            var bodyBytes = message.Body != null
                ? HsmsBody.ConverterToBytes(message.Body)
                : Array.Empty<byte>();
            int length = headerBytes.Length + bodyBytes.Length;
            var lens = BitConverter.GetBytes(length).Reverse().ToArray();
            return lens.Combine(headerBytes, bodyBytes);
        }

        public static string ConverterToSml(HsmsMessage message, bool addDescription = false)
        {
            var sb = new StringBuilder();
            sb.Append($"S{message.Header.Stream & 0x0F}F{message.Header.Function}");
            if (message.Header.Reply)
            {
                sb.Append(" W");
            }
            sb.Append("\r\n");
            if (addDescription && !string.IsNullOrWhiteSpace(message.Description))
            {
                sb.Append($" --{message.Description}");
            }
            if (message.Body != null)
            {
                string body = HsmsBody.ConverterToSml(message.Body, addDescription);
                sb.AppendLine(body);
            }
            sb.AppendLine(".");
            return sb.ToString().Trim();
        }
    }
}
