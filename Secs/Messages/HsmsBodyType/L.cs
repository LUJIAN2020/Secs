using Secs.Enums;
using System.Linq;

namespace Secs.Messages
{
    public class L : HsmsBody
    {
        public L()
        {
            Format = SecsFormat.L;
        }
        public L(int count) : this()
        {
            Count = count;
        }
        public L(params HsmsBody[] bodys) : this(bodys.Length)
        {
            SubBodys = bodys.ToList();
        }
        public L(string? description = default, params HsmsBody[] bodys) : this(bodys)
        {
            Description = description;
            SubBodys = bodys.ToList();
        }
    }
}
