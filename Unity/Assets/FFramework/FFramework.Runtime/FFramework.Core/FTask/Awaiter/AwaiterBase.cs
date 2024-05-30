using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace FFramework
{
    public abstract class AwaiterBase : FUnit, IFTaskAwaiter
    {

        public bool IsCompleted { get; protected set; } = false;

        protected System.Object m_ContinuationOrExceptionDispatchInfo = null;

        internal IFTask BindTask { get; set; } = null;

        protected FTaskStatus m_Status = FTaskStatus.Pending;
        public FTaskStatus Status
        {
            get => m_Status;
            internal set => m_Status = value;
        }


        protected FCancellationTokenHolder m_TokenHolder = null;
        public FCancellationTokenHolder TokenHolder => m_TokenHolder;

        protected AwaiterBase() { }

        void INotifyCompletion.OnCompleted(System.Action continuation)
        {
            this.m_ContinuationOrExceptionDispatchInfo = continuation;
            BindTask.Flow?.OnStart();
        }
        void ICriticalNotifyCompletion.UnsafeOnCompleted(System.Action continuation)
        {
            this.m_ContinuationOrExceptionDispatchInfo = continuation;
            BindTask.Flow?.OnStart();
        }


        void IFTaskAwaiter.SetToken(FCancellationTokenHolder token)
        {
            if (m_TokenHolder != null)
                Envirment.Current.GetModule<PoolModule>().Set<FCancellationTokenHolder, FCancellationTokenHolder.Poolable>(m_TokenHolder);
            m_TokenHolder = token;
        }
        public void SetFailed(ExceptionDispatchInfo exceptionDispatchInfo)
        {
            m_ContinuationOrExceptionDispatchInfo = exceptionDispatchInfo;
            m_Status = FTaskStatus.Failed;

            BindTask.Flow?.OnFailed();
            exceptionDispatchInfo.Throw();
        }

        public void SetCanceled()
        {
            if (m_Status.IsFinished())
                throw new System.InvalidOperationException(FTaskConst.FTASK_ALREADY_FINISHED_MESSAGE);

            m_Status = FTaskStatus.Canceled;
            IsCompleted = true;

            BindTask.Flow?.OnCancel();

            Recycle(BindTask);
        }

        public void SetSuspend()
        {
            if (m_Status.IsFinished())
                throw new System.InvalidOperationException(FTaskConst.FTASK_ALREADY_FINISHED_MESSAGE);

            BindTask.Flow?.OnSuspend();

            m_Status = FTaskStatus.Suspending;
        }

        public void SetRestore()
        {
            if (m_Status.IsFinished())
                throw new System.InvalidOperationException(FTaskConst.FTASK_ALREADY_FINISHED_MESSAGE);

            BindTask.Flow?.OnRestore();

            m_Status = FTaskStatus.Pending;
        }

        internal void Reset()
        {
            if (!m_Status.IsFinished())
                throw new System.InvalidOperationException(FTaskConst.FTASK_NOT_FINISHED_MESSAGE);

            IsCompleted = false;
            m_ContinuationOrExceptionDispatchInfo = null;
            m_Status = FTaskStatus.Pending;
            m_TokenHolder = null;
        }

        protected abstract void Recycle(IFTask task);
    }
}
