using System;

namespace FFramework
{
    /// <summary>
    /// 注意：如果手动调用了Dispose方法，那么需要手动释放其中的托管资源
    /// </summary>
    public abstract class FDispoableUnit : FUnit, IDisposable
    {
        private bool disposedValue;

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    OnReleaseManagedResource();
                }
                OnReleaseUnmanagedResource();
                disposedValue = true;
            }
        }

        /// <summary>
        /// 释放小型对象的引用，以及托管资源（此方法仅在通过Dispose调用时使用）
        /// </summary>
        protected abstract void OnReleaseManagedResource();
        /// <summary>
        /// 释放非托管资源，大型对象引用(此方法无论对象如何释放都会被执行)
        /// </summary>
        protected abstract void OnReleaseUnmanagedResource();

        ~FDispoableUnit()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
