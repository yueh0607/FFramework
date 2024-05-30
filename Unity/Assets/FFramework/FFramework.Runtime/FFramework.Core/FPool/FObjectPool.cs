using System.Collections;

namespace FFramework
{
    public abstract class FObjectPool : FDispoableUnit
    {
        protected Queue m_CachePool = new Queue();

        public int Count => m_CachePool.Count;
        public abstract object Get();

        public abstract void Set(object obj);

        public abstract void Release();

        protected override void OnReleaseManagedResource()
        {
            Release();
            m_CachePool = null;
        }

        protected override void OnReleaseUnmanagedResource()
        {
            
        }
    }


    public class FObjectPool<T> : FObjectPool
    {

        private IPoolable<T> m_TPoolable;

        public FObjectPool(IPoolable<T> poolable)
        {
            this.m_TPoolable = poolable;
        }

        public override object Get()
        {
            if (m_CachePool.Count == 0)
            {
                T t_obj = m_TPoolable.OnCreate();
                m_TPoolable.OnGet(t_obj);
                return t_obj;
            }
            object obj = m_CachePool.Dequeue();
            m_TPoolable.OnGet((T)obj);
            return obj;
        }

        public override void Release()
        {
            while (m_CachePool.Count != 0)
            {
                T obj = (T)m_CachePool.Dequeue();
                m_TPoolable.OnDestroy(obj);
            }
        }

        public override void Set(object obj)
        {
            m_TPoolable.OnSet((T)obj);
            if (obj is FUnit unit) unit.ResetID();
            if (m_CachePool.Count < m_TPoolable.Capacity)
                m_CachePool.Enqueue(obj);
            else
                m_TPoolable.OnDestroy((T)obj);
        }

        protected override void OnReleaseManagedResource()
        {
            base.OnReleaseManagedResource();
            m_TPoolable = null;
        }
    }
}

