using System;
using System.IO;

namespace AiFileOrganizer
{
    public static class FileLogger
    {
        private static string _logDir = "Logs";

        public static void Log(string message, string type = "INFO")
        {
            try
            {
                if (!Directory.Exists(_logDir)) Directory.CreateDirectory(_logDir);
                string fileName = Path.Combine(_logDir, $"Log_{DateTime.Now:yyyyMMdd}.txt");
                string logContent = $"[{DateTime.Now:HH:mm:ss}] [{type}] {message}{Environment.NewLine}";
                File.AppendAllText(fileName, logContent);
            }
            catch { }
        }

        public static void LogError(string message, Exception ex)
        {
            Log($"{message} | Error: {ex.Message}\nStack: {ex.StackTrace}", "ERROR");
        }
    }
}
