using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

namespace FFramework
{
    /// <summary>
    /// 自动回收资源句柄，如果主动释放将会回收，如果忘记主动释放，会产生GC
    /// </summary>
    public class UnityResourceHandle : ResourceHandle
    {
        private AssetHandle m_AssetHandle;


        bool managedResourceReleased = false;

        public override object GetHandle()=> m_AssetHandle;

        public override void SetHandle(object handle)=> m_AssetHandle = (AssetHandle)handle;

        protected override void OnReleaseManagedResource()
        {
            managedResourceReleased = true;
            FLoger.LogWarning($"Resource handle leaked and automatically released");
        }

        protected override void OnReleaseUnmanagedResource()
        {
            m_AssetHandle.Release();   
            //没执行析构函数，代表为主动释放，回收到池
            if(!managedResourceReleased)
            {
                ResourceHandle.ReleaseHandle<UnityResourceHandle, Poolable>(this);
            }
        }


        public class Poolable : IPoolable<UnityResourceHandle>
        {
            int IPoolable.Capacity => 1000;

            UnityResourceHandle IPoolable<UnityResourceHandle>.OnCreate()
            {
                return new UnityResourceHandle();
            }

            void IPoolable<UnityResourceHandle>.OnDestroy(UnityResourceHandle obj)
            {
                
            }

            void IPoolable<UnityResourceHandle>.OnGet(UnityResourceHandle obj)
            {
                
            }

            void IPoolable<UnityResourceHandle>.OnSet(UnityResourceHandle obj)
            {
                obj.managedResourceReleased = false;
                obj.m_AssetHandle = null;
            }
        }
    }
}
