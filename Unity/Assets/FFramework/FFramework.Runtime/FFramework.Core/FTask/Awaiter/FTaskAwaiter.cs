using System;

namespace FFramework
{
    public class FTaskAwaiter : AwaiterBase, IFTaskNotGenericAwaiter
    {
        public void GetResult() { OnGetResult(); }

        public void SetSucceed()
        {
            if (m_Status.IsFinished())
                throw new System.InvalidOperationException(FTaskConst.FTASK_ALREADY_FINISHED_MESSAGE);

            m_Status = FTaskStatus.Succeed;

            BindTask.Flow?.OnSucceed();
            ((Action)m_Continuation)?.Invoke();

            Recycle(BindTask);
        }

        protected override void Recycle(IFTask task)
            => Envirment.Current.GetModule<PoolModule>().Set<FTask, FTask.Poolable>((FTask)task);

        protected FTaskAwaiter() { }

        public class Poolable : IPoolable<FTaskAwaiter>
        {
            int IPoolable.Capacity => FTaskConst.NOT_GENERIC_FTASK_PER_TYPE_POOL_CAPACITY;

            FTaskAwaiter IPoolable<FTaskAwaiter>.OnCreate()
            {
                FTaskAwaiter m_Awaiter = new FTaskAwaiter();
                return m_Awaiter;
            }

            void IPoolable<FTaskAwaiter>.OnDestroy(FTaskAwaiter obj)
            {

            }

            void IPoolable<FTaskAwaiter>.OnGet(FTaskAwaiter obj)
            {
                obj.m_Status = FTaskStatus.Pending;
            }

            void IPoolable<FTaskAwaiter>.OnSet(FTaskAwaiter obj)
            {
                obj.ResetAwaiterExcludeStatus();
            }
        }
    }


    public class FTaskAwaiter<T> : AwaiterBase, IFTaskGenericAwaiter<T>
    {
        T m_Result;
        public T GetResult() => m_Result;

        public void SetSucceed(T result)
        {
            if (m_Status.IsFinished())
                throw new System.InvalidOperationException(FTaskConst.FTASK_ALREADY_FINISHED_MESSAGE);

            m_Result = result;

            m_Status = FTaskStatus.Succeed;

            BindTask.Flow?.OnSucceed();
            ((Action)m_Continuation)?.Invoke();

            Recycle(BindTask);
        }

        protected override void Recycle(IFTask task)
            => Envirment.Current.GetModule<PoolModule>().Set<FTask<T>, FTask<T>.Poolable>((FTask<T>)task);

        protected FTaskAwaiter() { }

        public class Poolable : IPoolable<FTaskAwaiter<T>>
        {
            int IPoolable.Capacity => FTaskConst.NOT_GENERIC_FTASK_PER_TYPE_POOL_CAPACITY;

            FTaskAwaiter<T> IPoolable<FTaskAwaiter<T>>.OnCreate()
            {
                FTaskAwaiter<T> m_Awaiter = new FTaskAwaiter<T>();
                return m_Awaiter;
            }

            void IPoolable<FTaskAwaiter<T>>.OnDestroy(FTaskAwaiter<T> obj)
            {

            }

            void IPoolable<FTaskAwaiter<T>>.OnGet(FTaskAwaiter<T> obj)
            {
                obj.m_Status = FTaskStatus.Pending;
            }

            void IPoolable<FTaskAwaiter<T>>.OnSet(FTaskAwaiter<T> obj)
            {
                obj.ResetAwaiterExcludeStatus();
                obj.m_Result = default;
            }
        }
    }
}