using System;
using YooAsset;

namespace FFramework
{
    public class UnityRawFileHandle : UnityResourceHandle<UnityRawFileHandle, UnityRawFileHandle.Poolable,RawFileHandle>
    {
        public byte[] GetRawFileData()=> m_AssetHandle.GetRawFileData();

        public string GetRawFileText()=> m_AssetHandle.GetRawFileText();


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
            else if (token != null && !token.IsCancellationRequested)
            {
                //保存状态
                token.Suspend();
                //恢复状态
                m_AssetHandle.Completed += (_) => token.Restore();
                await FTask.CompletedTask;
            }
        }

        public new class Poolable :
            UnityResourceHandle<UnityRawFileHandle, UnityRawFileHandle.Poolable, RawFileHandle>.Poolable,
            IPoolable<UnityRawFileHandle>
        {

            public new UnityRawFileHandle OnCreate()
            {
                return (UnityRawFileHandle)(base.OnCreate());
            }

            public void OnDestroy(UnityRawFileHandle obj)
            {
                base.OnDestroy(obj);
            }

            public void OnGet(UnityRawFileHandle obj)
            {
                base.OnGet(obj);
            }

            public void OnSet(UnityRawFileHandle obj)
            {
                base.OnSet(obj);
            }
        }



    }
}
