using System;

namespace FFramework
{
    public static class SingletonProperty<T> where T : class
    {
        private static bool m_initialized = false;

        private static readonly object locker = new object();

        private volatile static T instance = null;
        public static T Instance
        {
            get
            {
                if (!m_initialized)
                {
                    lock (locker)
                    {
                        if (!m_initialized)
                        {
                            m_initialized = true;
                            instance = Activator.CreateInstance<T>();
                        }
                    }
                }
                return instance;
            }
        }

        public static void Unload()
        {
            if (m_initialized)
            {
                lock (locker)
                {
                    if (m_initialized)
                    {
                        m_initialized = false;
                        instance = null;
                    }
                }
            }
            
        }
    }
}
