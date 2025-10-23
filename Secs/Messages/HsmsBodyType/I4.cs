using Secs.Enums;
using Secs.Extensions;

namespace Secs.Messages
{
    public class I4 : HsmsBody
    {
        public I4()
        {
            Format = SecsFormat.I4;
        }
        public I4(int[] values, string? description = default) : this()
        {
            Value = values;
            Count = values.Length;
            RawData = values.ToBytes(false);
            Description = description;
        }
        public I4(int value, string? description = default) : this(new int[] { value }, description)
        {
        }
    }
}
