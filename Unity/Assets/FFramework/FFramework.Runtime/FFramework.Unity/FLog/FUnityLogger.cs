using System.IO;
using UnityEngine;

namespace FFramework
{
    public class FUnityLogger : ILogger
    {
        public string GetLogPath()
        {
            return Path.Combine(Application.persistentDataPath, "FLog.log");
        }

        public void Log(ELogLevel lev, object message)
        {
            //log by unityEngine.Debug
            switch (lev)
            {
                case ELogLevel.Message:
                    UnityEngine.Debug.Log(message);
                    break;
                case ELogLevel.Warning:
                    UnityEngine.Debug.LogWarning(message);
                    break;
                case ELogLevel.Error:
                    UnityEngine.Debug.LogError(message);
                    break;
                case ELogLevel.Exception:
                    UnityEngine.Debug.LogException(message as System.Exception);
                    break;

                default:
                    throw new System.NotImplementedException($"Unsupported log type {lev} with content {message}");
            }
        }
    }
}
