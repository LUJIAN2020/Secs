using Secs.Enums;
using Secs.Extensions;

namespace Secs.Messages
{
    public class F4 : HsmsBody
    {
        public F4()
        {
            Format = SecsFormat.F4;
        }
        public F4(float[] values, string? description = default) : this()
        {
            Value = values;
            Count =  values.Length;
            RawData = values.ToBytes(false);
            Description = description;
        }
        public F4(float value, string? description = default) : this(new float[] { value }, description)
        {
        }
    }
}
