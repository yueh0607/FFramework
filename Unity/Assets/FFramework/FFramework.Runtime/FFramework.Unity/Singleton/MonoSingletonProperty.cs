using UnityEngine;

namespace FFramework
{
    public class MonoSingletonProperty<T> where T : MonoBehaviour
    {
        private static bool m_initialized = false;

        private static T m_Instance = null;

        public static T Instance
        {
            get
            {
                if(!m_initialized)
                {
                    GameObject singletonGameObject = new GameObject($"[MonoSingleton]{typeof(T).Name}");
                    GameObject.DontDestroyOnLoad(singletonGameObject);
                    m_Instance = singletonGameObject.AddComponent<T>();
                }
                return m_Instance;
            }
        }

        public static void Unload()
        {
            if (m_initialized)
            {
                GameObject.DestroyImmediate(m_Instance.gameObject);
                m_Instance = null;
                m_initialized = false;
            }
        }
    }
}
