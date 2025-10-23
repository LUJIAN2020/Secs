using Secs.Messages;
using System;

namespace Secs.Exceptions
{
    public class ReplyTimeoutException : Exception
    {
        public HsmsDataContext DataContext { get; }
        public ReplyTimeoutException(HsmsDataContext dataContext)
            : base(GetMessage(dataContext))
        {
            DataContext = dataContext;
        }
        private static string GetMessage(HsmsDataContext dataContext)
        {
            return $"Reply timeout T3, {dataContext}";
        }
    }
}
