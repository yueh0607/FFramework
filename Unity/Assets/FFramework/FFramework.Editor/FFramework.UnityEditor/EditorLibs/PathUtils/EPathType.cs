namespace FFramework
{
    public enum EPathType
    {
        AssetPath,  //Unity Assets·��
        ProjectPath, //Unity Assets·�����ϲ�·��������Asset��Libaray��
        RootPath,    //ProjectPath���ϲ�·����������������DotNet��Ŀ��Unity��Ŀ
        ScriptsPath, //Unity��ScriptsĿ¼
        AbsPath,    //����·��
        EditorSettingPath,  //�༭����Ŀ�����ļ��У�����Զ���ģ�����unity�ģ�
        StreamingAssetPath, 
        PersistentDataPath,
    }
}
