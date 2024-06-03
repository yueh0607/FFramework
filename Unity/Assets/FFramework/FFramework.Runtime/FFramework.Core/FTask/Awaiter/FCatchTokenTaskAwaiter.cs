using System;
namespace FFramework
{
    public class FCatchTokenTaskAwaiter : AwaiterBase, ISyncAwaiter, IFTaskAwaiter
    {

        public FCancellationTokenHolder GetResult()
        {
            return m_TokenHolder;
        }

        public void SetSucceed()
        {
            if (m_Status.IsFinished())
                throw new System.InvalidOperationException(FTaskConst.FTASK_ALREADY_FINISHED_MESSAGE);

            m_Status = FTaskStatus.Succeed;

            BindTask.Flow?.OnSucceed();
            ((Action)m_ContinuationOrExceptionDispatchInfo)?.Invoke();

            Recycle(BindTask);
        }


        protected override void Recycle(IFTask task)
        {
            Envirment.Current.GetModule<PoolModule>()
                .Set<FCatchTokenTask, FCatchTokenTask.Poolable>((FCatchTokenTask)task);
        }

        private FCatchTokenTaskAwaiter() { }

        public class Poolable : IPoolable<FCatchTokenTaskAwaiter>
        {
            int IPoolable.Capacity => FTaskConst.NOT_GENERIC_FTASK_PER_TYPE_POOL_CAPACITY;

            FCatchTokenTaskAwaiter IPoolable<FCatchTokenTaskAwaiter>.OnCreate()
            {
                FCatchTokenTaskAwaiter m_Awaiter = new FCatchTokenTaskAwaiter();
                return m_Awaiter;
            }

            void IPoolable<FCatchTokenTaskAwaiter>.OnDestroy(FCatchTokenTaskAwaiter obj)
            {

            }

            void IPoolable<FCatchTokenTaskAwaiter>.OnGet(FCatchTokenTaskAwaiter obj)
            {
                
            }

            void IPoolable<FCatchTokenTaskAwaiter>.OnSet(FCatchTokenTaskAwaiter obj)
            {
                obj.Reset();
            }
        }
    }
}
