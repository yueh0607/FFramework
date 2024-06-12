namespace FFramework
{
    public enum EPathType
    {
        AssetPath,  //Unity Assets路径
        ProjectPath, //Unity Assets路径的上层路径，包含Asset、Libaray等
        RootPath,    //ProjectPath的上层路径，包含分析器、DotNet项目、Unity项目
        ScriptsPath, //Unity的Scripts目录
        AbsPath,    //绝对路径
        EditorSettingPath,  //编辑器项目设置文件夹（框架自定义的，不是unity的）
        StreamingAssetPath, 
        PersistentDataPath,
    }
}
