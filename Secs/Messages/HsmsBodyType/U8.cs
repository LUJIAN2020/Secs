using Secs.Enums;
using Secs.Extensions;

namespace Secs.Messages
{
    public class U8 : HsmsBody
    {
        public U8()
        {
            Format = SecsFormat.U8;
        }
        public U8(ulong[] values, string? description = default) : this()
        {
            Value = values;
            Count = values.Length;
            RawData = values.ToBytes(false);
            Description = description;
        }
        public U8(ulong value, string? description = default) : this(new ulong[] { value }, description)
        {
        }
    }
}