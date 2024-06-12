using System;
using System.IO;
using UnityEngine;

namespace FFramework.HotFix.Editor
{
    public static class FolderBytesCopyer
    {

        /// <summary>
        /// 清空指定目录的文件
        /// </summary>
        /// <param name="path">绝对路径</param>
        public static void ClearFiles(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
                Directory.CreateDirectory(path);
            }

        }

        /// <summary>
        /// path为目录，filter为"*.dll",toPath为拷贝到目录，append
        /// 为”.bytes“
        /// </summary>
        /// <param name="path">绝对路径</param>
        /// <param name="filter">过滤器</param>
        public static int CopyBytes(string path, string filter, string toPath, string appendName = ".bytes", Predicate<string> specialFilter = null)
        {
            int count = 0;
            try
            {
                Directory.CreateDirectory(toPath);
                Directory.CreateDirectory(path);

                foreach (string file in Directory.EnumerateFiles(path))
                {
                    string fileName = System.IO.Path.GetFileName(file);
                    //Debug.Log(fileName);
                    if (fileName.EndsWith(filter)
                        && (specialFilter == null || specialFilter(System.IO.Path.GetFileNameWithoutExtension(fileName))))
                    {

                        string newFileName = fileName + appendName;
                        string destinationPath = System.IO.Path.Combine(toPath, newFileName);

                        File.Copy(file, destinationPath);
                        count++;
                    }
                }

                // 处理当前目录下的子目录
                foreach (string subDir in Directory.EnumerateDirectories(path))
                {
                    string subDirName = System.IO.Path.GetFileName(subDir);
                    string newSubDirPath = System.IO.Path.Combine(toPath, subDirName);
                    count += CopyBytes(subDir, filter, newSubDirPath, appendName, specialFilter); // 递归调用复制方法
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"Error occurred while copying files: {ex.Message}");
            }

            return count;
        }


    }
}