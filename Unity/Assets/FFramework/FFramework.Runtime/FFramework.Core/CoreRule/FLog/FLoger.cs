using System.IO;

namespace FFramework
{
    public static class FLoger
    {
        private static ILogger m_Logger;
        private static FLayerMask m_Filter = new FLayerMask();


        public static void SetLogger(ILogger logger)
        {
            m_Logger = logger;
        }

        /// <summary>
        /// 过滤日志设置
        /// </summary>
        /// <param name="lev"></param>
        /// <param name="isOpen"></param>
        public static void FilterLog(ELogLevel lev, bool isClose)
        {
            if (isClose)
                m_Filter = m_Filter >> (int)lev;
            else
                m_Filter = m_Filter << (int)lev;
        }


        public static void Log(object message)
        {
            if (m_Logger == null)
                return;

            m_Logger.Log(ELogLevel.Message, message);
        }

        public static void LogWarning(object message)
        {
            if (m_Logger == null)
                return;

            m_Logger.Log(ELogLevel.Warning, message);
        }

        public static void LogError(object message)
        {
            if (m_Logger == null)
                return;

            m_Logger.Log(ELogLevel.Error, message);
        }

        private static void WriteLog(ELogLevel lev, object message)
        {
            if (m_Logger == null)
                return;

            if (m_Filter != (int)lev)
            {
                using (StreamWriter sw = new StreamWriter(m_Logger.GetLogPath(), true))
                {
                    sw.WriteLine($"[{lev}] {message.ToString()}");
                }
            }
        }
    }
}
