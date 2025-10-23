using Secs.Enums;
using System;
using System.Text;

namespace Secs.Messages
{
    public class A : HsmsBody
    {
        public A()
        {
            Format = SecsFormat.A;
        }
        public A(string value, string? description = default) : this()
        {
            foreach (var c in value)
                if (c > 0x7F)
                    throw new Exception($"`{c}` is not ASCII char");

            Count = value.Length;
            Value = value;
            RawData = Encoding.ASCII.GetBytes(value);
            Description = description;
        }
    }
}
