using System.IO;
using UnityEngine;

namespace FFramework.Utils.Editor
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
                case EPathType.ProjectSettingPath:
                    {
                        return Path.Combine(GetLocation(EPathType.AssetPath), "FFramework/FFramework.Editor/FFramework.ProjectSetting");
                    }
                case EPathType.PreferencePath:
                    {
                        return Path.Combine(GetLocation(EPathType.ProjectPath), "FFramework/Preferences");
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

        public static bool FileExist(EPathType type, string relativePath)
        {
            return File.Exists(GetAbsLocation(type, relativePath));
        }

        public static void EnsureFileExist(EPathType type,string relativePath)
        {
            if(!FileExist(type,relativePath))
            {
                File.Create(GetAbsLocation(type, relativePath)).Dispose();
            }
        }

        public static bool DirectoryExist(EPathType type,string relativePath)
        {
            return Directory.Exists(GetAbsLocation(type, relativePath));
        }

        public static void EnsureDirectoryExist(EPathType type,string relativePath)
        {
            Directory.CreateDirectory(GetAbsLocation(type, relativePath));
        }

    }
}
