using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace FFramework
{
    public class ComponentGroup
    {
        [LabelText("BindObject")]
        public GameObject BindObject;

        [LabelText("Bind")]
        public List<Component> BindComponents = new List<Component>();

        
    }
}
