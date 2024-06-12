using UnityEditor;

namespace FFramework.HotFix.UnityEditor
{
    [FilePath("FFramework/Settings/HotFixToolSettings.asset",FilePathAttribute.Location.ProjectFolder)]
    public class HotFixToolSettings : ScriptableSingleton<HotFixToolSettings>
    {

        /// <summary>
        /// 热更新bytes拷贝路径
        /// </summary>
        public string hotFixBytesPath = "Assets/HotFix/HotUpdate";


        /// <summary>
        /// 元数据bytes拷贝路径
        /// </summary>
        public string metaDataBytesPath = "Assets/HotFix/MetaData";

        public string currentPlatform = string.Empty;


        public void Modify()
        {
            Save(true);
        }
    }
}