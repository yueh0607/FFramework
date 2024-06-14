using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FFramework
{
    public static class ModeHelper
    {

        #region ģʽ
#if UNITY_EDITOR
        public static ERunMode GetRunMode()
        {
            string modeStr = EditorPrefs.GetString("FFramework_RunMode", ERunMode.Simulated.ToString());
            return (ERunMode)Enum.Parse(typeof(ERunMode), modeStr);
        }
#else
        public static ERunMode GetRunMode()
        {
            return ERunMode.Real;
        }
#endif
        #endregion
    }
}
