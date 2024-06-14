using System;
using System.Threading;

namespace FFramework
{
    public class FSwitchThreadAwaiter : AwaiterBase, IFTaskNotGenericAwaiter, ISyncAwaiter
    {
        
        public void GetResult()
        {
            OnGetResult();
        }

        public void SetSucceed()
        {
            if (m_Status.IsFinished())
                throw new System.InvalidOperationException(FTaskConst.FTASK_ALREADY_FINISHED_MESSAGE);

            m_Status = FTaskStatus.Succeed;
           
            ((FSwitchThreadTask)BindTask).SwitchConext.Post(WaitCallback, null);
            BindTask.Flow?.OnSucceed();
            Recycle(BindTask);
        }

        void WaitCallback(object state)
        {
            ((Action)m_Continuation)?.Invoke();
        }

        protected override void Recycle(IFTask task)
        {

        }

        public class Poolable : IPoolable<FSwitchThreadAwaiter>
        {
            int IPoolable.Capacity => FTaskConst.NOT_GENERIC_FTASK_PER_TYPE_POOL_CAPACITY;

            FSwitchThreadAwaiter IPoolable<FSwitchThreadAwaiter>.OnCreate()
            {
                FSwitchThreadAwaiter m_Awaiter = new FSwitchThreadAwaiter();
                return m_Awaiter;
            }

            void IPoolable<FSwitchThreadAwaiter>.OnDestroy(FSwitchThreadAwaiter obj)
            {

            }

            void IPoolable<FSwitchThreadAwaiter>.OnGet(FSwitchThreadAwaiter obj)
            {
                obj.m_Status = FTaskStatus.Pending;
            }

            void IPoolable<FSwitchThreadAwaiter>.OnSet(FSwitchThreadAwaiter obj)
            {
                obj.ResetAwaiterExcludeStatus();
            }
        }
    }
}
