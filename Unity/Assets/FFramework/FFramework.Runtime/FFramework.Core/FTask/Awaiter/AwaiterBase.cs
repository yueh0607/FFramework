using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace FFramework
{
    public abstract class AwaiterBase : FUnit, IFTaskAwaiter
    {

        //可能被使用（表示当前MethodBuilder所等待的Awaiter）,也可能是null
        private IFTaskAwaiter m_CurrentAwaiter { get; set; }
        public IFTaskAwaiter CurrentAwaiter
        {
            get => m_CurrentAwaiter;
            set => m_CurrentAwaiter = value;
        }


        public void SetSyncSucceed()
        {
            IFTaskAwaiter curAwaiter = this;
            while (curAwaiter.CurrentAwaiter != null)
            {
                curAwaiter = curAwaiter.CurrentAwaiter;
            }

            if (curAwaiter is ISyncAwaiter syncAwaiter)
            {
                syncAwaiter.SetSucceed();
            }
        }


        public bool IsCompleted { get; private set; } = false;

        protected System.Object m_ContinuationOrExceptionDispatchInfo = null;

        internal IFTask BindTask { get; set; } = null;

        protected FTaskStatus m_Status = FTaskStatus.Pending;
        public FTaskStatus Status
        {
            get => m_Status;
            internal set => m_Status = value;
        }


        protected FCancellationToken m_TokenHolder = null;
        public FCancellationToken TokenHolder => m_TokenHolder;

        protected AwaiterBase() { }

        public void SetStarted()
        {
            BindTask.Flow?.OnStart();
        }

        void INotifyCompletion.OnCompleted(System.Action continuation)
        {
            this.m_ContinuationOrExceptionDispatchInfo = continuation;
          
        }
        void ICriticalNotifyCompletion.UnsafeOnCompleted(System.Action continuation)
        {
            this.m_ContinuationOrExceptionDispatchInfo = continuation;
        
        }


        void IFTaskAwaiter.SetToken(FCancellationToken token)
        {
            //if (m_TokenHolder != null)
            //Envirment.Current.GetModule<PoolModule>().Set<FCancellationTokenHolder, FCancellationTokenHolder.Poolable>(m_TokenHolder);
            m_TokenHolder = token;

            if (m_TokenHolder != null)
            {
                m_TokenHolder.Flow = BindTask.Flow;
            }

            if (CurrentAwaiter != null)
            {
                CurrentAwaiter.SetToken(token);
            }
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

            //只有Flow不是当前的令牌持有的,不是空，才能被调用，否则交给令牌调用
            if (TokenHolder != null && TokenHolder.Flow != BindTask.Flow && BindTask.Flow != null)
                BindTask.Flow.OnCancel();

            ((System.Action)m_ContinuationOrExceptionDispatchInfo)?.Invoke();
            Recycle(BindTask);
        }

        public void SetSuspend()
        {
            if (m_Status.IsFinished())
                throw new System.InvalidOperationException(FTaskConst.FTASK_ALREADY_FINISHED_MESSAGE);
            //只有Flow不是当前的令牌持有的,不是空，才能被调用，否则交给令牌调用
            if (TokenHolder != null && TokenHolder.Flow != BindTask.Flow && BindTask.Flow != null)
                BindTask.Flow.OnSuspend();

            m_Status = FTaskStatus.Suspending;
        }

        public void SetRestore()
        {
            if (m_Status.IsFinished())
                throw new System.InvalidOperationException(FTaskConst.FTASK_ALREADY_FINISHED_MESSAGE);
            //只有Flow不是当前的令牌持有的,不是空，才能被调用，否则交给令牌调用
            if (TokenHolder != null && TokenHolder.Flow != BindTask.Flow && BindTask.Flow != null)
                BindTask.Flow.OnRestore();

            m_Status = FTaskStatus.Pending;
        }

        internal void ResetAwaiterExcludeStatus()
        {
            if (!m_Status.IsFinished())
                throw new System.InvalidOperationException(FTaskConst.FTASK_NOT_FINISHED_MESSAGE);

            m_ContinuationOrExceptionDispatchInfo = null;

            if (TokenHolder != null && TokenHolder.Flow == BindTask.Flow)
                TokenHolder.Flow = null;
            m_TokenHolder = null;
        }

        protected abstract void Recycle(IFTask task);


    }
}
