using Secs.Enums;
using Secs.Extensions;

namespace Secs.Messages
{
    public class U2 : HsmsBody
    {
        public U2()
        {
            Format = SecsFormat.U2;
        }
        public U2(ushort[] values, string? description = default) : this()
        {
            Value = values;
            Count = values.Length;
            RawData = values.ToBytes(false);
            Description = description;
        }
        public U2(ushort value, string? description = default) : this(new ushort[] { value }, description)
        {
        }
    }
}
