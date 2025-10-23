using Secs.Enums;
using Secs.Extensions;

namespace Secs.Messages
{
    public class F8 : HsmsBody
    {
        public F8()
        {
            Format = SecsFormat.F8;
        }
        public F8(double[] values, string? description = default) : this()
        {
            Value = values;
            Count = values.Length;
            RawData = values.ToBytes(false);
            Description = description;
        }
        public F8(double value, string? description = default) : this(new double[] { value }, description)
        {
        }
    }
}
