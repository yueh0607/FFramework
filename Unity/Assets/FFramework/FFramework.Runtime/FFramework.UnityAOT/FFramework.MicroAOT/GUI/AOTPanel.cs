using UnityEngine;

namespace FFramework
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class AOTPanel : MonoBehaviour
    {
        public string Location { get; internal set; } = string.Empty;

        public int Key { get; internal set; } = 0;

        public abstract void OnShow();

        public abstract void OnClose();

        public virtual void OnRefocus() { }
    }
}
