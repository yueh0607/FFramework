using System.Diagnostics;

namespace FFramework
{
    public abstract class FPoolUnit<T> : FDispoableUnit where T : FPoolUnit<T>
    {

        bool m_ManagedResourceReleased = false;
        protected override void OnReleaseManagedResource()
        {
            m_ManagedResourceReleased = true;
        }

        protected override void OnReleaseUnmanagedResource()
        {
            FPool.Set<T>((T)this);
        }
    }
}
