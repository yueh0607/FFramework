using System.Threading;
using System.Threading.Tasks;

namespace FFramework
{
    public class ThreadingPromise : IFTaskFlow
    {
        public Task InvokeTask { get; set; }

        public ISucceedCallback BindTask { get; set; }

        public CancellationTokenSource BindToken { get; set; }

        SynchronizationContext context;
        void IFTaskFlow.OnCancel()
        {
            if (BindToken == null)
                throw new System.NullReferenceException("BindToken is null");
            BindToken.Cancel();
        }

        void IFTaskFlow.OnFailed()
        {

        }

        void IFTaskFlow.OnRestore()
        {
            throw new System.InvalidOperationException("Threading Task is not allowed to suspend");
        }

        void IFTaskFlow.OnStart()
        {
            context = SynchronizationContext.Current;
            RunTask(InvokeTask);
        }

        void IFTaskFlow.OnSucceed()
        {

        }

        void IFTaskFlow.OnSuspend()
        {
            throw new System.InvalidOperationException("Threading Task is not allowed to suspend");
        }

        async void RunTask(Task task)
        {
            await task;
            if (BindToken != null && BindToken.IsCancellationRequested) return;
            //在主线程找合适的时机完成
            context.Post((state) =>
            {
                if (BindToken != null && BindToken.IsCancellationRequested) return;
                BindTask.SetSucceed();
            }, null);
        }


        public class Poolable : IPoolable<ThreadingPromise>
        {
            int IPoolable.Capacity => FTaskConst.NOT_GENERIC_FTASK_PER_TYPE_POOL_CAPACITY;

            ThreadingPromise IPoolable<ThreadingPromise>.OnCreate()
            {
                return new ThreadingPromise();
            }

            void IPoolable<ThreadingPromise>.OnDestroy(ThreadingPromise obj)
            {

            }

            void IPoolable<ThreadingPromise>.OnGet(ThreadingPromise obj)
            {

            }

            void IPoolable<ThreadingPromise>.OnSet(ThreadingPromise obj)
            {
                obj.InvokeTask = null;
                obj.BindTask = null;
                obj.BindToken = null;
                obj.context = null;
            }
        }
    }



    public class ThreadingPromise<T> : IFTaskFlow
    {
        public Task<T> InvokeTask { get; set; }

        public ISucceedCallback<T> BindTask { get; set; }

        public CancellationTokenSource BindToken { get; set; }

        SynchronizationContext context;
        void IFTaskFlow.OnCancel()
        {
            if (BindToken == null)
                throw new System.NullReferenceException("BindToken is null");
            BindToken.Cancel();
        }

        void IFTaskFlow.OnFailed()
        {

        }

        void IFTaskFlow.OnRestore()
        {
            throw new System.InvalidOperationException("Threading Task is not allowed to suspend");
        }

        void IFTaskFlow.OnStart()
        {
            context = SynchronizationContext.Current;
            RunTask(InvokeTask);
        }

        void IFTaskFlow.OnSucceed()
        {

        }

        void IFTaskFlow.OnSuspend()
        {
            throw new System.InvalidOperationException("Threading Task is not allowed to suspend");
        }

        async void RunTask(Task<T> task)
        {
            T result = await task;
            if (BindToken != null && BindToken.IsCancellationRequested) return;
            //在主线程找合适的时机完成
            context.Post((state) =>
            {
                if (BindToken != null && BindToken.IsCancellationRequested) return;
                BindTask.SetSucceed(result);
            }, null);
        }


        public class Poolable : IPoolable<ThreadingPromise<T>>
        {
            int IPoolable.Capacity => FTaskConst.GENERIC_FTASK_PER_TYPE_POOL_CAPACITY;

            ThreadingPromise<T> IPoolable<ThreadingPromise<T>>.OnCreate()
            {
                return new ThreadingPromise<T>();
            }

            void IPoolable<ThreadingPromise<T>>.OnDestroy(ThreadingPromise<T> obj)
            {

            }

            void IPoolable<ThreadingPromise<T>>.OnGet(ThreadingPromise<T> obj)
            {

            }

            void IPoolable<ThreadingPromise<T>>.OnSet(ThreadingPromise<T> obj)
            {
                obj.InvokeTask = null;
                obj.BindTask = null;
                obj.BindToken = null;
                obj.context = null;
            }
        }
    }
}



