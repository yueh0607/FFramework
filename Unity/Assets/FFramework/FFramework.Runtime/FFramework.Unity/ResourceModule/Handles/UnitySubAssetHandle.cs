using System;
using YooAsset;

namespace FFramework
{
    public class UnitySubAssetHandle: UnityResourceHandle<UnitySubAssetHandle, UnitySubAssetHandle.Poolable, SubAssetsHandle>
    {


        public override async FTask EnsureDone(IProgress<float> progress = null)
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
            UnityResourceHandle<UnitySubAssetHandle, UnitySubAssetHandle.Poolable, SubAssetsHandle>.Poolable,
            IPoolable<UnitySubAssetHandle>
        {

            public new UnitySubAssetHandle OnCreate()
            {
                return (UnitySubAssetHandle)(base.OnCreate());
            }

            public void OnDestroy(UnitySubAssetHandle obj)
            {
                base.OnDestroy(obj);
            }

            public void OnGet(UnitySubAssetHandle obj)
            {
                base.OnGet(obj);
            }

            public void OnSet(UnitySubAssetHandle obj)
            {
                base.OnSet(obj);
            }
        }



    }
}
