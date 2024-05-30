using System;

namespace FFramework
{
    internal abstract class ProviderInstance<T> : IProvider where T : ProviderInstance<T>, new()
    {
        [ThreadStatic]
        static T m_Instance = null;


        public static T Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new T();
                return m_Instance;
            }
        }

        public abstract unsafe void Deserialize<K>(void* source, void* address);
        public abstract unsafe void Serialize<K>(void* destination, void* address);
    }
}
