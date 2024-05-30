namespace FFramework
{
    internal partial class LockPromise
    {
        public class UnlockPromise : FUnit, IFTaskFlow
        {
            public object LockedAsset { get; set; } = null;
            public ISucceedCallback BindTask { get; set; } = null;

            void IFTaskFlow.OnCancel()
            {
                //取消视作成功，执行解锁操作
                LockPromise.m_LockedSet.Remove(LockedAsset);
                Envirment.Current.GetModule<EventModule>()
                    .Publisher.SendAll<IAsyncLockChanged>(LockedAsset);
                Envirment.Current.GetModule<PoolModule>().Set<UnlockPromise, UnlockPromise.Poolable>(this);
            }

            void IFTaskFlow.OnFailed()
            {

            }

            void IFTaskFlow.OnRestore()
            {

            }

            void IFTaskFlow.OnStart()
            {
                m_LockedSet.Remove(LockedAsset);
                Envirment.Current.GetModule<EventModule>()
                    .Publisher.SendAll<IAsyncLockChanged>(LockedAsset);
                BindTask.SetSucceed();
            }

            void IFTaskFlow.OnSucceed()
            {
                Envirment.Current.GetModule<PoolModule>().Set<UnlockPromise, UnlockPromise.Poolable>(this);
            }

            void IFTaskFlow.OnSuspend()
            {

            }

            public class Poolable : IPoolable<UnlockPromise>
            {
                int IPoolable.Capacity => 1000;

                UnlockPromise IPoolable<UnlockPromise>.OnCreate()
                {
                    return new UnlockPromise();
                }

                void IPoolable<UnlockPromise>.OnDestroy(UnlockPromise obj)
                {

                }

                void IPoolable<UnlockPromise>.OnGet(UnlockPromise obj)
                {

                }

                void IPoolable<UnlockPromise>.OnSet(UnlockPromise obj)
                {
                    obj.BindTask = null;
                    obj.LockedAsset = null;
                }
            }
        }
    }

}
