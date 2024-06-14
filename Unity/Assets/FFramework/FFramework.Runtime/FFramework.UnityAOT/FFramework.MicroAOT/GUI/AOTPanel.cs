using System.Collections;
using UnityEngine;

namespace FFramework
{

    public abstract class AOTPanel : MonoBehaviour
    {
        public string Location { get; internal set; } = string.Empty;

        public int Key { get; internal set; } = 0;

        public abstract IEnumerator OnShow();

        public abstract IEnumerator OnClose();

    }
}
