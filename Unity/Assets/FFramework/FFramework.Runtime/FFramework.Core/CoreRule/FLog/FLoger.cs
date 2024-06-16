using System;
using System.IO;
using System.Text;
using UnityEditor.VersionControl;

namespace FFramework
{
    public static class FLoger
    {
        private static ILogger m_Logger;
        private static FLayerMask m_LogFilter = new FLayerMask(FLayerMask.AllOpenedValue);
        private static FLayerMask m_WriteFilter = new FLayerMask(FLayerMask.AllOpenedValue);

        public static void SetLogger(ILogger logger)
        {
            m_Logger = logger;

            if (!Path.HasExtension(m_Logger.GetLogPath()))
                throw new System.ArgumentException("log path must be a file");

            Directory.CreateDirectory(Path.GetDirectoryName(m_Logger.GetLogPath()));

            if (!File.Exists(m_Logger.GetLogPath()))
                File.Create(m_Logger.GetLogPath()).Dispose();

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                if (m_Logger == null)
                    return;
                if (m_LogFilter != (int)ELogLevel.Exception)
                    return;

                m_Logger.Log(ELogLevel.Exception, e.ExceptionObject);

                if (m_WriteFilter != (int)ELogLevel.Exception)
                    return;
                WriteLog(ELogLevel.Exception, e.ExceptionObject);
            };
        }

        /// <summary>
        /// 过滤日志设置
        /// </summary>
        /// <param name="lev"></param>
        /// <param name="isOpen"></param>
        public static void FilterLog(ELogLevel lev, bool isClose)
        {

            if (isClose)
                m_LogFilter = m_LogFilter >> (int)lev;
            else
                m_LogFilter = m_LogFilter << (int)lev;
        }


        public static void Log(object message)
        {
            if (m_Logger == null)
                return;
            if (m_LogFilter != (int)ELogLevel.Message)
                return;


            m_Logger.Log(ELogLevel.Message, message);

            if (m_WriteFilter != (int)ELogLevel.Message)
                return;
            WriteLog(ELogLevel.Message, message);
        }

        public static void LogWarning(object message)
        {
            if (m_Logger == null)
                return;
            if (m_LogFilter != (int)ELogLevel.Warning)
                return;
            m_Logger.Log(ELogLevel.Warning, message);

            if (m_WriteFilter != (int)ELogLevel.Warning)
                return;
            WriteLog(ELogLevel.Warning, message);
        }

        public static void LogError(object message)
        {
            if (m_Logger == null)
                return;
            if (m_LogFilter != (int)ELogLevel.Error)
                return;


            m_Logger.Log(ELogLevel.Error, message);


            if (m_WriteFilter != (int)ELogLevel.Error)
                return;
            WriteLog(ELogLevel.Error, message);
        }

        private static void WriteLog(ELogLevel lev, object message)
        {
            if (m_Logger == null)
                return;


            using (StreamWriter sw = new StreamWriter(m_Logger.GetLogPath(), true, Encoding.UTF8))
            {
                sw.WriteLine(m_Logger.CreateFormatFileWrite(lev,message));
            }

        }
    }
}
