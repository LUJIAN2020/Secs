using Secs.Enums;
using Secs.Extensions;

namespace Secs.Messages
{
    public class I2 : HsmsBody
    {
        public I2()
        {
            Format = SecsFormat.I2;
        }
        public I2(short[] values, string? description = default) : this()
        {
            Value = values;
            Count = values.Length;
            RawData = values.ToBytes(false);
            Description = description;
        }
        public I2(short value, string? description = default) : this(new short[] { value }, description)
        {
        }
    }
}
