using System.IO;
using System.Reflection;
using UnityEditor;

namespace FFramework.Utils.Editor
{
    
    public class EditorDataSingleton<T> where T : EditorDataSingleton<T>, new()
    {
        private static readonly object m_Lock = new object();
        private static volatile T m_Instance = null;
        private static FilePathAttribute m_FilePath = null;
        public static T Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    lock (m_Lock)
                    {
                        if (m_Instance == null)
                        {
                            ReadOrNew();
                        }
                    }
                }
                return m_Instance;

            }
        }

        private const string JSON_EXTENSION = ".json";

        private static string FilePath => m_FilePath.AbsPath + JSON_EXTENSION;

        public static void Save()
        {
            string json = EditorJsonUtility.ToJson(m_Instance,prettyPrint:true);
            File.WriteAllText(FilePath, json);
            AssetDatabase.Refresh();
        }

        private static void ReadOrNew()
        {
            if (System.IO.File.Exists(FilePath))
            {
                string allText = File.ReadAllText(FilePath);
                m_Instance = new T();
                EditorJsonUtility.FromJsonOverwrite(allText, m_Instance);
            }
            else
            {
                m_Instance = new T();
            }
        }

        public static void Reset()
        {
            if (System.IO.File.Exists(FilePath))
            {
                File.Delete(FilePath);
                m_Instance = null;
                AssetDatabase.Refresh();
            }
        }


        static EditorDataSingleton()
        {
            var filePathAtt = typeof(T).GetCustomAttribute<FilePathAttribute>()
                ?? throw new System.InvalidProgramException($"{typeof(T).FullName} must have a {typeof(FilePathAttribute).FullName}");
            m_FilePath = filePathAtt;
        }


        protected EditorDataSingleton() { }
    }
}