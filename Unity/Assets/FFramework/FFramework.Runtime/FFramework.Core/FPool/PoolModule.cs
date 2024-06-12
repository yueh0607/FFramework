using System;
using System.Collections.Generic;


namespace FFramework
{
    [ModuleStatic("FPool")]
    public class PoolModule : IModule
    {

        private Dictionary<Type, FObjectPool> m_CachePools = new Dictionary<Type, FObjectPool>();

        internal object InternalGet(Type type)
        {
            if (!m_CachePools.ContainsKey(type))
                throw new InvalidOperationException("No such pool");

            return m_CachePools[type].Get();
        }
        public T Get<T>() where T : class
        {
            return (T)InternalGet(typeof(T));
        }

        public T Get<T, K>() where T : class where K : IPoolable<T>, new()
        {
            Create(new K());
            return (T)m_CachePools[typeof(T)].Get();
        }

        internal void InternalSet(Type type, object obj)
        {
            if (!m_CachePools.ContainsKey(type))
                throw new InvalidOperationException("No such pool");
            if (obj == null) throw new NullReferenceException();

            m_CachePools[type].Set(obj);
        }

        public void Set<T>(T obj) where T : class
        {
            InternalSet(typeof(T), obj);
        }

        public void Set<T, K>(T obj) where T : class where K : IPoolable<T>, new()
        {
            Create(new K());
            if (obj == null)
            {
                throw new NullReferenceException("You are tring to set a null");
            }
            m_CachePools[typeof(T)].Set(obj);

        }

        public int Count<T>() where T : class
        {
            if (!m_CachePools.ContainsKey(typeof(T)))
                throw new InvalidOperationException("No such pool");

            return m_CachePools[typeof(T)].Count;
        }

        public int Count<T, K>() where T : class where K : IPoolable<T>, new()
        {
            Create(new K());
            return m_CachePools[typeof(T)].Count;
        }


        public void Release<T>() where T : class
        {
            if (!m_CachePools.ContainsKey(typeof(T)))
                throw new InvalidOperationException("No such pool");

            m_CachePools[typeof(T)].Dispose();
            m_CachePools.Remove(typeof(T));
        }

        public void TryRelease<T>() where T : class
        {
            if (m_CachePools.ContainsKey(typeof(T)))
            {
                m_CachePools[typeof(T)].Dispose();
                m_CachePools.Remove(typeof(T));
            }
        }

        public void Create<T>(IPoolable<T> properties) where T : class
        {
            if (!m_CachePools.ContainsKey(typeof(T)))
                m_CachePools.Add(typeof(T), new FObjectPool<T>(properties));
        }

        void IModule.OnCreate(object moduleParameter)
        {

        }

        void IModule.OnDestroy()
        {
            foreach (var pool in m_CachePools)
            {
                pool.Value.Dispose();
            }
            m_CachePools.Clear();
            m_CachePools = null;
        }
    }
}
