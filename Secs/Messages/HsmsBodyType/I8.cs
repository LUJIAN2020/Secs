using Secs.Enums;
using Secs.Extensions;

namespace Secs.Messages
{
    public class I8 : HsmsBody
    {
        public I8()
        {
            Format = SecsFormat.I8;
        }
        public I8(long[] values, string? description = default) : this()
        {
            Value = values;
            Count = values.Length;
            RawData = values.ToBytes(false);
            Description = description;
        }
        public I8(long value, string? description = default) : this(new long[] { value }, description)
        {
        }
    }
}
