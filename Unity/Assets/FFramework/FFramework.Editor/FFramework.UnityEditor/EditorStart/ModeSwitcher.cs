using FFramework;
using System;
using UnityEditor;

namespace UnityToolbarExtender.Examples
{


    [InitializeOnLoad]
    public sealed class ModeSwitchLeftButton
    {



        static ModeSwitchLeftButton()
        {

            ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
        }
        const string MODE_KEY = "FFramework_RunMode";
        static void OnToolbarGUI()
        {

            string modeStr = EditorPrefs.GetString(MODE_KEY, ERunMode.Simulated.ToString());
            if (Enum.TryParse(typeof(ERunMode), modeStr, out object mode))
            {
                ERunMode runMode = (ERunMode)EditorGUILayout.EnumPopup((ERunMode)mode);

                if (runMode.ToString() != modeStr)
                {
                    EditorPrefs.SetString(MODE_KEY, runMode.ToString());
                }
            }
            else EditorPrefs.SetString(MODE_KEY, ERunMode.Simulated.ToString());

          
        }
    }


}