using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

namespace FFramework.MicroAOT
{
    public sealed class AOTStartInfoManager  : AOTSingleton<AOTStartInfoManager>
    {

        const string INFO_NAME = "StartInfo.INI";
   
        private static XmlDocument m_Doc;
        private static XmlNode m_Root;
        public static XmlNode Root
        {
            get
            {
                if (m_Doc == null)
                {
                    m_Doc = new XmlDocument();
                    using (StreamReader reader = new StreamReader(Path.Combine(Application.streamingAssetsPath, INFO_NAME)))
                    {
                        string xmlText = reader.ReadToEnd();
                        m_Doc.LoadXml(xmlText);
                    }
                    m_Root = m_Doc.SelectSingleNode("Root");
                }

                return m_Root;
            }
        }

        static string GetItem(string name)
        {
            var node = Root[nameof(GameLogicPackageName)];
            if (node == null)
                throw new System.InvalidOperationException($"StartInfo/Root/{name} is not existed");
            return node.InnerText;
        }

        public static string GameLogicPackageName => GetItem(nameof(GameLogicPackageName));

        public static string GameResourcePackageName => GetItem(nameof(GameResourcePackageName));

        public static string PatchMetaDataDllTag => GetItem(nameof(PatchMetaDataDllTag));

        public static string HotUpdateDllTag => GetItem(nameof(HotUpdateDllTag));

        public static string HotUpdateEntryClass => GetItem(nameof(HotUpdateEntryClass));

        public static string HotUpdateEntryMethod => GetItem(nameof(HotUpdateEntryMethod));
    }
}
