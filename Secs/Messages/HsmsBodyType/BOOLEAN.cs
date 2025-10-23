using Secs.Enums;
using System.Linq;

namespace Secs.Messages
{
    public class BOOLEAN : HsmsBody
    {
        public BOOLEAN()
        {
            Format = SecsFormat.BOOLEAN;
        }
        public BOOLEAN(bool[] values, string? description = default) : this()
        {
            Count = values.Length;
            Value = values;
            RawData = values.Select(c => (byte)(c ? 1 : 0)).ToArray();
            Description = description;
        }
        public BOOLEAN(bool value, string? description = default) : this(new bool[] { value }, description)
        {
        }
    }
}
