using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Component = UnityEngine.Component;
namespace FFramework
{

    [UnityEngine.DisallowMultipleComponent]
    public class ScriptMark : MonoBehaviour
    {

#if UNITY_EDITOR

        public List<UnityEngine.Component> buildComponents;
        public List<string> buildProperties;

        //生成View
        public string GenerateName = string.Empty;
        public string ViewPath = "Scripts/Views";
        public string ViewName = string.Empty;
        public string ViewBaseType = "FFramework.View";
        public string ViewNameSpace = "FFramework.Views";

        public string ViewDescription = string.Empty;

        void Reset()
        {
           
            GenerateName = FormatToVariableName(gameObject.name);
            ViewName = FormatToVariableName(gameObject.name);
        }

   
        string FormatToVariableName(string name)
        {
            string pattern = "[^a-zA-Z0-9_\u4e00-\u9fa5]";
            return Regex.Replace(name, pattern, string.Empty);
        }
#endif

    }
}
