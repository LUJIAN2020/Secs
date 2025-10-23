using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Secs.Demo.Services
{
    public class LogService : ILogService
    {
        private readonly static object _lock = new object();
        private string GetFilePath()
        {
            var now = DateTime.Now;
            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return Path.Combine(dir, $"{now:yyyy-MM-dd}.log");
        }
        private void WriteFile(string path, string text)
        {
            lock (_lock)
            {
                using (var sw = new StreamWriter(path, true, Encoding.UTF8))
                {
                    sw.WriteLine(text);
                }
            }
            Debug.WriteLine(text);
        }
        public void Info(string msg)
        {
            msg = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - INFO - {msg}";
            string fileName = GetFilePath();
            WriteFile(fileName, msg);
        }
        public void Error(Exception ex, string? msg = default)
        {
            if (msg == null)
            {
                Error(ex);
            }
            else
            {
                msg = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - ERROR - {msg}\r\n{ex.StackTrace}";
                string fileName = GetFilePath();
                WriteFile(fileName, msg);
            }
        }
        public void Error(Exception ex)
        {
            string msg = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - ERROR - {ex.StackTrace}";
            string fileName = GetFilePath();
            WriteFile(fileName, msg);
        }
    }
}
