using System;

namespace Secs.Demo.Services
{
    public interface ILogService
    {
        void Info(string msg);
        void Error(Exception ex, string? msg = default);
        void Error(Exception ex);
    }
}
