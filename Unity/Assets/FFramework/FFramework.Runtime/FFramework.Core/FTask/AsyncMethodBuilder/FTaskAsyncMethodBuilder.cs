using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
namespace FFramework
{
    public struct FTaskAsyncMethodBuilder
    {
        private FTask m_FTask;
        public readonly FTask Task => m_FTask;
        private bool m_NotFirstAwait;

        public static FTaskAsyncMethodBuilder Create()
        {
            return new FTaskAsyncMethodBuilder() { m_FTask = Envirment.Current.GetModule<PoolModule>().Get<FTask, FTask.Poolable>() };
        }

        public readonly void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        {
            stateMachine.MoveNext();
        }

        public readonly void SetStateMachine(IAsyncStateMachine stateMachine)
        {
            // do nothing
        }

        public readonly void SetResult()
        {
            ((ISucceedCallback)m_FTask.GetAwaiter()).SetSucceed();
        }

        public readonly void SetException(System.Exception exception)
        {
            m_FTask.GetAwaiter().SetFailed(ExceptionDispatchInfo.Capture(exception));
        }


        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine
        {
            if (!(awaiter is IFTaskAwaiter))
                throw new System.InvalidOperationException($"Awaiter must implement interface {nameof(IFTaskAwaiter)}");

            IFTaskAwaiter fTaskAwaiter = (IFTaskAwaiter)awaiter;

            //TODO:如果传入的有令牌，则不能覆盖
            if (fTaskAwaiter.TokenHolder == null)              
                fTaskAwaiter.SetToken(m_FTask.GetAwaiter().TokenHolder);
            m_FTask.GetAwaiter().CurrentAwaiter = fTaskAwaiter;

            if (fTaskAwaiter.TokenHolder != null && fTaskAwaiter.TokenHolder.Token.IsCancellationRequested)
            {
                fTaskAwaiter.SetCanceled();
            }
            else if (fTaskAwaiter.TokenHolder != null && fTaskAwaiter.TokenHolder.Token.IsSuspendRequested)
            {
                fTaskAwaiter.SetSuspend();
                fTaskAwaiter.TokenHolder.Token.InternalRegisterSuspendCallback(stateMachine.MoveNext, fTaskAwaiter.SetRestore);
            }
            else if (fTaskAwaiter.Status == FTaskStatus.Pending)
            {
                fTaskAwaiter.OnCompleted(stateMachine.MoveNext);
                if (m_NotFirstAwait)
                {
                    fTaskAwaiter.SetSyncSucceed();
                }
                else m_NotFirstAwait = true;
            }


        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine
            => AwaitOnCompleted(ref awaiter, ref stateMachine);


    }


    public struct FTaskAsyncMethodBuilder<T>
    {
        private FTask<T> m_FTask;
        public readonly FTask<T> Task => m_FTask;
        private bool m_NotFirstAwait;
        public static FTaskAsyncMethodBuilder<T> Create()
        {
            return new FTaskAsyncMethodBuilder<T>() { m_FTask = Envirment.Current.GetModule<PoolModule>().Get<FTask<T>, FTask<T>.Poolable>() };
        }

        public readonly void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        {
            stateMachine.MoveNext();
        }

        public readonly void SetStateMachine(IAsyncStateMachine stateMachine)
        {
            // do nothing
        }

        public readonly void SetResult(T result)
        {
            ((ISucceedCallback<T>)m_FTask.GetAwaiter()).SetSucceed(result);
        }

        public readonly void SetException(System.Exception exception)
        {
            m_FTask.GetAwaiter().SetFailed(ExceptionDispatchInfo.Capture(exception));
        }

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine
        {
            if (!(awaiter is IFTaskAwaiter))
                throw new System.InvalidOperationException($"Awaiter must implement interface {nameof(IFTaskAwaiter)}");

            IFTaskAwaiter fTaskAwaiter = (IFTaskAwaiter)awaiter;

            //TODO:释放旧Token,设置新Token
            fTaskAwaiter.SetToken(m_FTask.GetAwaiter().TokenHolder);
            m_FTask.GetAwaiter().CurrentAwaiter = fTaskAwaiter;


            if (fTaskAwaiter.TokenHolder != null && fTaskAwaiter.TokenHolder.Token.IsCancellationRequested)
            {
                fTaskAwaiter.SetCanceled();
            }
            else if (fTaskAwaiter.TokenHolder != null && fTaskAwaiter.TokenHolder.Token.IsSuspendRequested)
            {
                fTaskAwaiter.SetSuspend();
                fTaskAwaiter.TokenHolder.Token.InternalRegisterSuspendCallback(stateMachine.MoveNext, fTaskAwaiter.SetRestore);
            }
            else if (fTaskAwaiter.Status == FTaskStatus.Pending)
            {
                fTaskAwaiter.OnCompleted(stateMachine.MoveNext);
                if (m_NotFirstAwait)
                {
                    fTaskAwaiter.SetSyncSucceed();
                }
                else m_NotFirstAwait = true;

            }


        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine
            => AwaitOnCompleted(ref awaiter, ref stateMachine);

    }
}