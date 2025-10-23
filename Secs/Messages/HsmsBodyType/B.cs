using Secs.Enums;

namespace Secs.Messages
{
    public class B : HsmsBody
    {
        public B()
        {
            Format = SecsFormat.B;
        }
        public B(byte[] values, string? description = default) : this()
        {
            Count = values.Length;
            Value = values;
            RawData = values;
            Description = description;
        }
        public B(byte value, string? description = default) : this(new byte[] { value }, description)
        {
        }
    }
}
