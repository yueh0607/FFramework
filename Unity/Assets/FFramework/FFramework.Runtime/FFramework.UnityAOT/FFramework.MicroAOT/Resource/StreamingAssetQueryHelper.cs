using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FFramework.MicroAOT
{
    public sealed class StreamingAssetQueryHelper
    {
        public const string YOO_ASSET_STREAMING_ROOT = "yoo";

        //filePath、crc32 
        private static Dictionary<string, string> m_ExistCache = new Dictionary<string, string>();
        public static void Init() { }
        public static bool FileExists(string packageName, string fileName, string fileCRC)
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, YOO_ASSET_STREAMING_ROOT, packageName, fileName);

            //已经确认过此文件存在
            if (m_ExistCache.TryGetValue(filePath, out string cacheCRC))
            {
                //如果已经确认的文件CRC和当前请求文件一致
                if (cacheCRC == fileCRC)
                    return true;
            }
            //第一次尝试访问此文件
            else
            {
                //如果文件存在
                if (File.Exists(filePath))
                {
                    string crc32 = YooAsset.HashUtility.FileCRC32(filePath);
                    m_ExistCache.Add(filePath, crc32);
                    if (crc32 == fileCRC) return true;
                }
                //文件不存在
                else
                {
                    return false;
                }
            }
            return false;

        }
    }

}