using System.IO;
using UnityEngine;

namespace FFramework
{



    public static class EditorPathUtils
    {
        private static string m_ProjectPath = null;
        private static string m_RootPath = null;
        public static string GetLocation(EPathType type)
        {
            switch (type)
            {
                case EPathType.AssetPath: return Application.dataPath;
                case EPathType.PersistentDataPath: return Application.persistentDataPath;
                case EPathType.StreamingAssetPath: return Application.streamingAssetsPath;
                case EPathType.RootPath:
                    {
                        m_RootPath ??= new System.IO.DirectoryInfo(GetLocation(EPathType.ProjectPath)).Parent.FullName;
                        return m_ProjectPath;
                    }
                case EPathType.ProjectPath:
                    {
                        m_ProjectPath ??= new System.IO.DirectoryInfo(GetLocation(EPathType.AssetPath)).Parent.FullName;
                        return m_ProjectPath;
                    }
                case EPathType.EditorSettingPath:
                    {
                        return Path.Combine(GetLocation(EPathType.AssetPath), "FFramework/FFramework.Editor/FFramework.ProjectSetting");
                    }
                case EPathType.ScriptsPath:
                    {
                        return Path.Combine(GetLocation(EPathType.AssetPath), "Scripts");
                    }
                default: return string.Empty;

            }
        }


        public static string GetAbsLocation(EPathType type, string relativePath)
        {
            return Path.Combine(GetLocation(type), relativePath);
        }
    }
}
