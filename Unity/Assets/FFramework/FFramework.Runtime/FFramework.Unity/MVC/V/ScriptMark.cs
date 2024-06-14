using System.Collections.Generic;
using UnityEngine;
namespace FFramework
{

    public class ScriptMark : MonoBehaviour
    {

#if UNITY_EDITOR

        public List<Component> buildComponents;
        public List<string> buildProperties;       

        //生成View
        public string ViewPath = "Scripts/Views";
        public string ViewName = string.Empty;
        public string ViewBaseType = "FFramework.View";
        public string ViewNameSpace = "FFramework.Views";

        public string ViewDescription = string.Empty;


#endif

    }
}
