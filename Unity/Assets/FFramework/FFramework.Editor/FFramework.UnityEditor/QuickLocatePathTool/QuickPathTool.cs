using System.IO;
using UnityEditor;
using UnityEngine;

namespace FFramework.QuickPath.Editor
{
    public class QuickPathTool 
    {

        [MenuItem(itemName: "StreamingAssets", menuItem = "FFramework/QuickPath/StreamingAssets Path")]
        static void Open1()
        {
            OpenURL(Application.streamingAssetsPath);
        }
        [MenuItem(itemName: "PeristentData", menuItem = "FFramework/QuickPath/PersistentData Path")]
        static void Open2()
        {
            OpenURL(Application.persistentDataPath);
        }
        [MenuItem(itemName: "Data", menuItem = "FFramework/QuickPath/Data Path")]
        static void Open3()
        {
            OpenURL(Application.dataPath);
        }

        [MenuItem(itemName: "Project", menuItem = "FFramework/QuickPath/Project Path")]
        static void Open4()
        {
            OpenURL(new DirectoryInfo(Application.dataPath).Parent.FullName);
        }

        [MenuItem(itemName: "UnityEditor", menuItem = "FFramework/QuickPath/UnityEditor Path")]
        static void Open5()
        {
            OpenURL(EditorApplication.applicationPath);
        }
        [MenuItem(itemName: "EditorContents", menuItem = "FFramework/QuickPath/EditorContents Path")]
        static void Open6()
        {
            OpenURL(EditorApplication.applicationContentsPath);
        }
        [MenuItem(itemName: "ConsoleLog", menuItem = "FFramework/QuickPath/ConsoleLog Path")]
        static void Open7()
        {
            OpenURL(Application.consoleLogPath);
        }
        [MenuItem(itemName: "TempCahce", menuItem = "FFramework/QuickPath/TemporaryCache Path")]
        static void Open8()
        {
            OpenURL(Application.temporaryCachePath);
        }

        static void OpenURL(string path)
        {
            Application.OpenURL(path);
        }

    }
}
