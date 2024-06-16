using System;

namespace FFramework
{


    public interface ILogger
    {
        void Log(ELogLevel lev, object message);

        string GetLogPath();

        string CreateFormatFileWrite(ELogLevel lev, object message)
        {
            return $"[{lev} {DateTime.Now}] {message}";
        }
    }
}
