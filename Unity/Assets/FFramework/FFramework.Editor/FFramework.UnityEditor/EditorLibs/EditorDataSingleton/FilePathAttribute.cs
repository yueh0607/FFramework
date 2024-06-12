using System;

namespace FFramework.Utils.Editor
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class FilePathAttribute : Attribute
    {
        private string m_FilePath;
        private EPathType m_PathType;

        public string AbsPath => EditorPathUtils.GetAbsLocation(m_PathType, m_FilePath);



        public FilePathAttribute(string relativePath, EPathType pathType = EPathType.AssetPath)
        {
            this.m_FilePath = relativePath;
            this.m_PathType = pathType;
        }
    }
}
