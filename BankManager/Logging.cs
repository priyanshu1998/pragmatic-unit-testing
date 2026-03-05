using System.IO;

namespace BankManager
{
    public static class Logging
    {
        private static ILogger _logger;
        public static ILogger Logger
        {
            get { return _logger ?? (_logger = new FileSystemLogger(@"C:\BankLog.txt")); }
            set { _logger = value; }
        }

        public static void WriteLine(string message)
        {
            Logger.WriteLine(message);
        }
    }

    public interface ILogger
    {
        void WriteLine(string message);
    }

    public class FileSystemLogger : ILogger
    {
        private readonly string _logPath;
        public FileSystemLogger(string logPath)
        {
            _logPath = logPath;
            if (!File.Exists(_logPath))
                File.Create(_logPath);
        }

        public void WriteLine(string message)
        {
            using (var writer = File.AppendText(_logPath))
                writer.WriteLine(message);
        }
    }
}
