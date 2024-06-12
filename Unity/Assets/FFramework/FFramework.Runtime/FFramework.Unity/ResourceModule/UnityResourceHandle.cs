using System;
using YooAsset;

namespace FFramework
{
    /// <summary>
    /// 自动回收资源句柄，如果主动释放将会回收，如果忘记主动释放，会产生GC
    /// </summary>
    public abstract class UnityResourceHandle<T, K, U> : ResourceHandle
        where T : UnityResourceHandle<T, K, U>, new() where K : class, IPoolable<T>, new() where U : HandleBase
    {

        protected U m_AssetHandle;


        bool managedResourceReleased = false;


        public bool IsDone => m_AssetHandle.IsDone;

        public override void SetHandle(object handle) => m_AssetHandle = (U)handle;
        public override object GetHandle() => m_AssetHandle;

        protected override void OnReleaseManagedResource()
        {
            //资源句柄自动释放，执行析构，产生垃圾，发出提示
            managedResourceReleased = true;
            FLoger.LogWarning($"Resource handle leaked and automatically released");
        }

        protected override void OnReleaseUnmanagedResource()
        {
            //没执行析构函数，代表为主动释放，回收到池
            if (!managedResourceReleased)
            {
                ResourceHandle.ReleaseHandle<T, K>((T)this);
            }
        }

        public async virtual FTask EnsureDone(IProgress<float> progress = null)
        {
            await FTask.CompletedTask;
            if (m_AssetHandle.IsDone)
            {
                return;
            }
            await FTask.WaitUntil
            (
                () =>
                {
                    progress?.Report(m_AssetHandle.Progress);
                    return m_AssetHandle.IsDone;
                }
            );
        }


        public class Poolable : IPoolable<UnityResourceHandle<T, K, U>>
        {
            int IPoolable.Capacity => 1000;

            public UnityResourceHandle<T, K, U> OnCreate()
            {
                return new T();
            }

            public void OnDestroy(UnityResourceHandle<T, K, U> obj)
            {

            }

            public void OnGet(UnityResourceHandle<T, K, U> obj)
            {

            }

            public void OnSet(UnityResourceHandle<T, K, U> obj)
            {
                obj.managedResourceReleased = false;
                obj.SetHandle(null);
            }
        }


    }



}
