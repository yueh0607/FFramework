using FFramework.Utils.Editor;

namespace FFramework.HotFix.Editor
{
    [FilePath("HotFixToolSettings",EPathType.ProjectSettingPath)]
    public class HotFixToolSettings : EditorDataSingleton<HotFixToolSettings>
    {

        /// <summary>
        /// 热更新bytes拷贝路径
        /// </summary>
        public string hotFixBytesPath = "Res/GameLogic/HotUpdateDlls";


        /// <summary>
        /// 补充元数据bytes拷贝路径
        /// </summary>
        public string metaDataBytesPath = "Res/GameLogic/PatchMetaDataDlls";


    }
}