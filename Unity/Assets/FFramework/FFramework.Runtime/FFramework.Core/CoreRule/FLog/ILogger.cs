namespace FFramework
{


    public interface ILogger
    {
        void Log(ELogLevel lev, object message);

        string GetLogPath();

    }
}
