using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace FFramework
{
    public interface IFTaskAwaiter : ICriticalNotifyCompletion,IAsyncMachineCurrent
    {
        bool IsCompleted { get; }

        FCancellationTokenHolder TokenHolder { get; }

        FTaskStatus Status { get; }

        void SetSyncSucceed();
        
        void SetFailed(ExceptionDispatchInfo exceptionDispatchInfo);

        void SetCanceled();

        void SetSuspend();

        void SetRestore();

        /// <summary>
        /// TODO: 检查并释放原TokenHolder，设置新的TokenHolder
        /// </summary>
        /// <param name="token"></param>
        void SetToken(FCancellationTokenHolder token);
    }

    public interface IFTaskNotGenericAwaiter : IFTaskAwaiter,ISucceedCallback
    {
        void GetResult();

    }

    public interface IFTaskGenericAwaiter<T> : IFTaskAwaiter,ISucceedCallback<T>
    {
        T GetResult();

    }
}