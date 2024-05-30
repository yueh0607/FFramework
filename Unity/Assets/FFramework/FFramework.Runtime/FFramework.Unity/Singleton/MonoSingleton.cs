using UnityEngine;

namespace FFramework
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        protected MonoSingleton() { }


        public static T Instance => MonoSingletonProperty<T>.Instance;

        public static void Unload()=> MonoSingletonProperty<T>.Unload();
    }
}
