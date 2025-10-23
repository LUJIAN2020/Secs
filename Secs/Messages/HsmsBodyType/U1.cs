using Secs.Enums;

namespace Secs.Messages
{
    public class U1 : HsmsBody
    {
        public U1()
        {
            Format = SecsFormat.U1;
        }
        public U1(byte[] values, string? description = default) : this()
        {
            Value = values;
            Count = values.Length;
            RawData = values;
            Description = description;
        }
        public U1(byte value, string? description = default) : this(new byte[] { value }, description)
        {
        }
    }
}
