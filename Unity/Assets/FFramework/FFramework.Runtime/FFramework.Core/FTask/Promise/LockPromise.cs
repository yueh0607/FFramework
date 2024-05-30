using System.Collections.Generic;

namespace FFramework
{
    public interface IAsyncLockChanged : ISendEvent<object>
    {
        void OnAsyncUnLock(object asset);
    }
    internal partial class LockPromise : FUnit, IFTaskFlow, IAsyncLockChanged
    {
        private readonly static HashSet<object> m_LockedSet = new HashSet<object>();

        //被锁定的资源
        public object LockedAsset { get; set; }

        public ISucceedCallback BindTask { get; set; }

        void IFTaskFlow.OnCancel()
        {
            m_LockedSet.Remove(LockedAsset);
            Envirment.Current.GetModule<PoolModule>().Set<LockPromise, LockPromise.Poolable>(this);
        }

        void IFTaskFlow.OnFailed()
        {

        }

        void IFTaskFlow.OnRestore()
        {

        }

        void IFTaskFlow.OnStart()
        {
            if (!m_LockedSet.Contains(LockedAsset))
            {
                m_LockedSet.Add(LockedAsset);
                BindTask.SetSucceed();
            }
        }

        void IFTaskFlow.OnSucceed()
        {
            Envirment.Current.GetModule<PoolModule>().Set<LockPromise, LockPromise.Poolable>(this);
        }

        void IFTaskFlow.OnSuspend()
        {

        }

        //当锁内容发生变化时
        void IAsyncLockChanged.OnAsyncUnLock(object asset)
        {
            if (asset == LockedAsset)
            {
                BindTask.SetSucceed();
            }
        }


        public class Poolable : IPoolable<LockPromise>
        {
            int IPoolable.Capacity => 1000;

            LockPromise IPoolable<LockPromise>.OnCreate()
            {
                return new LockPromise();
            }

            void IPoolable<LockPromise>.OnDestroy(LockPromise obj)
            {

            }

            void IPoolable<LockPromise>.OnGet(LockPromise obj)
            {
                Envirment.Current.GetModule<EventModule>()
                    .Publisher.Subscribe<IAsyncLockChanged>(obj);
            }

            void IPoolable<LockPromise>.OnSet(LockPromise obj)
            {
                Envirment.Current.GetModule<EventModule>()
                    .Publisher.UnSubscribe<IAsyncLockChanged>(obj);
                obj.LockedAsset = null;
                obj.BindTask = null;
            }
        }
    }
}
