using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFM.Common.Lib
{
    public class DbLog<T> : IDbLogger<T>
    {
        private readonly Logger logger;
        public DbLog()
        {
            // Sử dụng tên của lớp chứa logger để tạo logger theo kiểu động.
            logger = LogManager.GetLogger(typeof(T).FullName);
        }

        public void LogDebbug(string message)
        {
           
            logger.Debug(message);
        }

        public void LogError(Exception ex, string message)
        {
           
            logger.Error(ex,message);
        }

        public void LogInfo(string message)
        {
            throw new NotImplementedException();
        }

        public void LogWarning(string message)
        {
            throw new NotImplementedException();
        }


        private LogEventInfo LogToDatabase(LogLevel logLevel, string message)
        {
            var logEvent = new LogEventInfo(logLevel, logger.Name,message);
            return logEvent;
        }
    }
}
