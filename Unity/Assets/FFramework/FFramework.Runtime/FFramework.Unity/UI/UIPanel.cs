using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace FFramework
{
    public class UIPanel : MonoBehaviour, IView
    {
        [LabelText("组件")]
        public List<ComponentGroup> Components = new List<ComponentGroup>();
    }
}
