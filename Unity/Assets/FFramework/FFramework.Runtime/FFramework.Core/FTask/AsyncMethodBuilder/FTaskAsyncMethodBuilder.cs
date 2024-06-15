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
        }

        public readonly void SetResult()
        {
            m_FTask.GetAwaiter().CurrentAwaiter = null;
            ((ISucceedCallback)m_FTask.GetAwaiter()).SetSucceed();
        }

        public readonly void SetException(System.Exception exception)
        {
            m_FTask.GetAwaiter().SetFailed(ExceptionDispatchInfo.Capture(exception));
        }


        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine
        {
            //排除任何不能兼容的Awaiter
            if (!(awaiter is IFTaskAwaiter))
                throw new System.InvalidOperationException($"Awaiter must implement interface {nameof(IFTaskAwaiter)}");

            IFTaskAwaiter fTaskAwaiter = (IFTaskAwaiter)awaiter;

            //TODO:如果传入的有令牌，则不能成功覆盖令牌，阻断令牌传递流
            if (fTaskAwaiter.TokenHolder == null)
                fTaskAwaiter.SetToken(m_FTask.GetAwaiter().TokenHolder);

            //设置当前状态机所等待的的Awaiter
            m_FTask.GetAwaiter().CurrentAwaiter = fTaskAwaiter;
            //指定下一步的行为
            fTaskAwaiter.OnCompleted(stateMachine.MoveNext);


            //有令牌，且有取消请求
            if (fTaskAwaiter.TokenHolder != null && fTaskAwaiter.TokenHolder.IsCancellationRequested)
            {
                fTaskAwaiter.SetCanceled();
            }
            //有令牌，且有挂起请求
            else if (fTaskAwaiter.TokenHolder != null && fTaskAwaiter.TokenHolder.IsSuspendRequested)
            {
                fTaskAwaiter.SetSuspend();
                fTaskAwaiter.TokenHolder.InternalRegisterSuspendCallback(fTaskAwaiter.SetStarted, fTaskAwaiter.SetRestore);
            }
            //无令牌或处于响应状态
            else if (fTaskAwaiter.Status == FTaskStatus.Pending)
            {
                fTaskAwaiter.SetStarted();
                //TODO：根据CurrentAwaiter链进行查找，找到最深的任务，如果是同步任务则完成
                if (m_NotFirstAwait)
                {
                    fTaskAwaiter.SetSyncSucceed();
                }
            }
            m_NotFirstAwait = true;
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
            m_FTask.GetAwaiter().CurrentAwaiter = null;
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
            fTaskAwaiter.OnCompleted(stateMachine.MoveNext);

            if (fTaskAwaiter.TokenHolder != null && fTaskAwaiter.TokenHolder.IsCancellationRequested)
            {
                fTaskAwaiter.SetCanceled();
            }
            else if (fTaskAwaiter.TokenHolder != null && fTaskAwaiter.TokenHolder.IsSuspendRequested)
            {
                fTaskAwaiter.SetSuspend();
                fTaskAwaiter.TokenHolder.InternalRegisterSuspendCallback(fTaskAwaiter.SetStarted, fTaskAwaiter.SetRestore);
            }
            else if (fTaskAwaiter.Status == FTaskStatus.Pending)
            {
                fTaskAwaiter.SetStarted();
                if (m_NotFirstAwait)
                {
                    fTaskAwaiter.SetSyncSucceed();
                }
            }

            m_NotFirstAwait = true;
        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine
            => AwaitOnCompleted(ref awaiter, ref stateMachine);

    }
}