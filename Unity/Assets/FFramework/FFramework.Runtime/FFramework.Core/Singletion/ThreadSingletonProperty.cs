using System;

namespace FFramework
{
    public static class ThreadSingletonProperty<T> where T : class
    {
        [ThreadStatic]
        private static bool m_initialized = false;

        [ThreadStatic]
        private static T instance = null;

        public static T Instance
        {
            get
            {
                if (!m_initialized)
                {
                    m_initialized = true;
                    instance = Activator.CreateInstance<T>();
                }
                return instance;
            }
        }

        public static void Unload()
        {
            if (m_initialized)
            {
                m_initialized = false;
                instance = null;
            }
        }
    }
}

