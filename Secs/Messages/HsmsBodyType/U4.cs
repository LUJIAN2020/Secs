using Secs.Enums;
using Secs.Extensions;

namespace Secs.Messages
{
    public class U4 : HsmsBody
    {
        public U4()
        {
            Format = SecsFormat.U4;
        }
        public U4(uint[] values, string? description = default) : this()
        {
            Value = values;
            Count = values.Length;
            RawData = values.ToBytes(false);
            Description = description;
        }
        public U4(uint value, string? description = default) : this(new uint[] { value }, description)
        {
        }
    }
}
