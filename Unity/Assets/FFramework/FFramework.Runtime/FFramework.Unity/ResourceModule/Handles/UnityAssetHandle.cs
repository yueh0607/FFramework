using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

namespace FFramework
{
    public class UnityAssetHandle : UnityResourceHandle<UnityAssetHandle, UnityAssetHandle.Poolable, AssetHandle>
    {
        public UnityEngine.Object AssetObject => base.m_AssetHandle.AssetObject;

        public T GetAssetObject<T>() where T : UnityEngine.Object => (T)AssetObject;



        public async FTask EnsureDone(IProgress<float> progress = null)
        {
            FCancellationToken token = await FTask.CatchToken();
            if (m_AssetHandle.IsDone)
            {
                return;
            }
            ///如果需要进度
            if (progress != null)
            {
                await FTask.WaitUntil
                (
                    () =>
                    {
                        progress.Report(m_AssetHandle.Progress);
                        return m_AssetHandle.IsDone;
                    }
                );
            }
            //不需要进度，且有令牌,可以不用更新
            else if (token != null)
            {
                //保存状态
                token.Suspend();
                //恢复状态
                m_AssetHandle.Completed += (_) => token.Restore();
                await FTask.CompletedTask;
            }

        }

        public new class Poolable :
            UnityResourceHandle<UnityAssetHandle, UnityAssetHandle.Poolable, AssetHandle>.Poolable,
            IPoolable<UnityAssetHandle>
        {

            public new UnityAssetHandle OnCreate()
            {
                return (UnityAssetHandle)(base.OnCreate());
            }

            public void OnDestroy(UnityAssetHandle obj)
            {
                base.OnDestroy(obj);
            }

            public void OnGet(UnityAssetHandle obj)
            {
                base.OnGet(obj);
            }

            public void OnSet(UnityAssetHandle obj)
            {
                base.OnSet(obj);

            }
        }



    }
}
