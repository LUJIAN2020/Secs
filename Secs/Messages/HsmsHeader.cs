using Secs.Enums;
using System;

namespace Secs.Messages
{
    /// <summary>
    /// Header information
    /// </summary>
    public class HsmsHeader
    {
        public HsmsHeader(ushort sessionId, byte stream, byte function, SType sType, int systemBytes, bool reply = true)
        {
            SessionId = sessionId;
            Stream = stream;
            Function = function;
            PType = 0; // Fixed for HSMS
            SType = sType;
            SystemBytes = systemBytes;
            Reply = reply;
        }

        public HsmsHeader(byte[] buffer)
        {
            if (buffer.Length != 10)
                throw new ArgumentOutOfRangeException(nameof(buffer.Length), buffer.Length, "Data length must be 10 bytes");

            SessionId = (ushort)((buffer[0] << 8) + buffer[1]);
            Stream = buffer[2];
            Reply = (buffer[2] >> 4) == 8;
            Function = buffer[3];
            PType = buffer[4];
            SType = (SType)buffer[5];
            SystemBytes = (buffer[6] << 24) + (buffer[7] << 16) + (buffer[8] << 8) + buffer[9];
        }

        /// <summary>
        /// Equipment identifier
        /// </summary>
        public ushort SessionId { get; } = 0xFFFF;

        /// <summary>
        /// Stream number
        /// </summary>
        public byte Stream { get; }

        /// <summary>
        /// Function code: sender sends odd numbers, responder sends even numbers
        /// </summary>
        public byte Function { get; }

        /// <summary>
        /// Message encoding type, fixed to 0 for HSMS
        /// </summary>
        public byte PType { get; }

        /// <summary>
        /// Session type
        /// </summary>
        public SType SType { get; }

        /// <summary>
        /// Communication identifier for a single transaction
        /// </summary>
        public int SystemBytes { get; }

        /// <summary>
        /// true = must reply, false = no reply needed; it is recommended that all messages request a reply
        /// </summary>
        public bool Reply { get; set; }

        /// <summary>
        /// Command string
        /// </summary>
        public string Command => $"S{Stream & 0x0F}F{Function}";
        public static byte[] ConverterToBytes(HsmsHeader header)
        {
            byte stream = 0;
            if (header.Stream != 0 && header.Function != 0)
            {
                stream = header.Reply
                    ? (byte)((header.Stream & 0x0F) + 0x80)
                    : header.Stream;

                if ((header.Stream & 0xF0) == 0x80)
                {
                    header.Reply = true;
                }
            }
            return new byte[]
            {
                (byte)(header.SessionId>>8),
                (byte)header.SessionId,
                stream,
                header.Function,
                header.PType,
                (byte)header.SType,
                (byte)(header.SystemBytes>>24),
                (byte)(header.SystemBytes>>16),
                (byte)(header.SystemBytes>>8),
                (byte)header.SystemBytes
            };
        }
        public static HsmsHeader CreateDefaultReplyHsmsHeader(HsmsHeader header)
        {
            ushort sessionId = header.SessionId;
            byte stream = (byte)(header.Stream & 0x0F);
            byte Function = (byte)(header.Function + 1);
            var sType = header.SType;
            var sb = header.SystemBytes;
            return new HsmsHeader(sessionId, stream, Function, sType, sb, false);
        }
    }
}
