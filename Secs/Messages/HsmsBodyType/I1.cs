using Secs.Enums;
using System.Linq;

namespace Secs.Messages
{
    public class I1 : HsmsBody
    {
        public I1()
        {
            Format = SecsFormat.I1;
        }
        public I1(sbyte[] values, string? description = default) : this()
        {
            Value = values;
            Count = values.Length;
            RawData = values.Select(c => (byte)c).ToArray();
            Description = description;
        }
        public I1(sbyte value, string? description = default) : this(new sbyte[] { value }, description)
        {
        }
    }
}
