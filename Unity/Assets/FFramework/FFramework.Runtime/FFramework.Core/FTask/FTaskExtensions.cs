using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace FFramework
{
    public partial class FTask
    {
        public static FCatchTokenTask CompletedTask => Envirment.Current.GetModule<PoolModule>().Get<FCatchTokenTask, FCatchTokenTask.Poolable>();

        public static FTask Delay(TimeSpan delayTime,ETimerLoop loop=ETimerLoop.Update)
        {
            DelayPromise timer = Envirment.Current.GetModule<PoolModule>().Get<DelayPromise, DelayPromise.Poolable>();
            FTask task = Envirment.Current.GetModule<PoolModule>().Get<FTask, FTask.Poolable>();
            timer.BindTask = task.GetAwaiter();
            timer.TargetTime = (float)delayTime.TotalSeconds;
            timer.TimerLoop = loop;
            task.m_TaskFlow = timer;
            return task;
        }

        public static FTask WaitUntil(Func<bool> predicate, ETimerLoop loop = ETimerLoop.Update)
        {
            WaitUntilPromise wait = Envirment.Current.GetModule<PoolModule>().Get<WaitUntilPromise, WaitUntilPromise.Poolable>();
            FTask task = Envirment.Current.GetModule<PoolModule>().Get<FTask, FTask.Poolable>();
            wait.WaitCondition = predicate;
            wait.BindTask = task.GetAwaiter();
            wait.TimerLoop = loop;
            task.m_TaskFlow = wait;
            wait.WhileMode = false;
            return task;
        }

        public static FTask WaitWhile(Func<bool> predicate, ETimerLoop loop = ETimerLoop.Update)
        {
            WaitUntilPromise wait = Envirment.Current.GetModule<PoolModule>().Get<WaitUntilPromise, WaitUntilPromise.Poolable>();
            FTask task = Envirment.Current.GetModule<PoolModule>().Get<FTask, FTask.Poolable>();
            wait.WaitCondition = predicate;
            wait.BindTask = task.GetAwaiter();
            wait.TimerLoop = loop;
            task.m_TaskFlow = wait;
            wait.WhileMode = true;

            return task;
        }

        public static async FTask WhenAny(params FTask[] tasks)
        {
            if (tasks.Length < 1)
                throw new ArgumentOutOfRangeException("tasks", "tasks is empty");

            WaitCountPromise promise = Envirment.Current.GetModule<PoolModule>().Get<WaitCountPromise, WaitCountPromise.Poolable>();
            FTask task = Envirment.Current.GetModule<PoolModule>().Get<FTask, FTask.Poolable>();
            promise.WaitCount = 1;
            promise.WaitArray = tasks;
            promise.BindTask = task.GetAwaiter();
            task.m_TaskFlow = promise;

            FCancellationTokenHolder tokenHolder = await FTask.CatchToken();
            promise.TokenHolder = tokenHolder;
        }

        public static async FTask WhenAll(params FTask[] tasks)
        {
            if (tasks.Length == 0) return;

            WaitCountPromise promise = Envirment.Current.GetModule<PoolModule>().Get<WaitCountPromise, WaitCountPromise.Poolable>();
            FTask task = Envirment.Current.GetModule<PoolModule>().Get<FTask, FTask.Poolable>();
            promise.WaitCount = tasks.Length;
            promise.WaitArray = tasks;
            promise.BindTask = task.GetAwaiter();
            task.m_TaskFlow = promise;

            FCancellationTokenHolder tokenHolder = await FTask.CatchToken();
            promise.TokenHolder = tokenHolder;
        }

        //public static FTask<T> WhenAny<T>(params FTask<T>[] tasks)
        //{

        //}

        //public static FTask<T[]> WhenAll<T>(params FTask<T>[] tasks)
        //{

        //}

        public static FCatchTokenTask CatchToken()
        {
            return Envirment.Current.GetModule<PoolModule>().Get<FCatchTokenTask, FCatchTokenTask.Poolable>();
        }

        public static FTask LockAsset(object asset)
        {
            LockPromise lockPromise = Envirment.Current.GetModule<PoolModule>().Get<LockPromise, LockPromise.Poolable>();
            FTask task = Envirment.Current.GetModule<PoolModule>().Get<FTask, FTask.Poolable>();
            lockPromise.LockedAsset = asset;
            lockPromise.BindTask = task.GetAwaiter();
            task.m_TaskFlow = lockPromise;

            return task;
        }

        public static FTask UnlockAsset(object asset)
        {
            LockPromise.UnlockPromise unlockPromise = Envirment.Current.GetModule<PoolModule>().Get<LockPromise.UnlockPromise, LockPromise.UnlockPromise.Poolable>();
            FTask task = Envirment.Current.GetModule<PoolModule>().Get<FTask, FTask.Poolable>();
            unlockPromise.LockedAsset = asset;
            unlockPromise.BindTask = task.GetAwaiter();
            task.m_TaskFlow = unlockPromise;
            return task;
        }

        public static FSwitchThreadTask SwitchThread(FSynchronizationContext context)
        {
            FSwitchThreadTask fSwitchThreadTask = Envirment.Current.GetModule<PoolModule>().Get<FSwitchThreadTask, FSwitchThreadTask.Poolable>();
            fSwitchThreadTask.SwitchConext = context;
            return fSwitchThreadTask;
        }
    }



    public static class FTaskExtensions
    {

        public static void Forget(this FTask task, FCancellationTokenHolder source)
        {
            ((IFTaskAwaiter)task.GetAwaiter()).SetToken(source);
            task.CurrentAwaiter.SetToken(source);
            Forget(task);
        }
        public static void Forget(this FTask task)
        {
            if (task.CurrentAwaiter != null && task.CurrentAwaiter is ISyncAwaiter syncAwaiter)
            {
                syncAwaiter.SetSucceed();
            }
        }
        public static void Forget<T>(this FTask<T> task, FCancellationTokenHolder tokenHolder)
        {
            ((IFTaskAwaiter)task.GetAwaiter()).SetToken(tokenHolder);
            task.CurrentAwaiter.SetToken(tokenHolder);
            Forget<T>(task);
        }
        public static void Forget<T>(this FTask<T> task)
        {
            if (task.CurrentAwaiter != null && task.CurrentAwaiter is ISyncAwaiter syncAwaiter)
            {
                syncAwaiter.SetSucceed();
            }
        }
        public static FTask ToFTask(this Task task, CancellationTokenSource tokenSource = null)
        {
            ThreadingPromise promise = Envirment.Current.GetModule<PoolModule>().Get<ThreadingPromise, ThreadingPromise.Poolable>();
            FTask fTask = Envirment.Current.GetModule<PoolModule>().Get<FTask, FTask.Poolable>();
            promise.InvokeTask = task;
            promise.BindTask = fTask.GetAwaiter();
            promise.BindToken = tokenSource;
            fTask.m_TaskFlow = promise;
            return fTask;
        }

        public static FTask<TResult> ToFTask<TResult>(this Task<TResult> task, CancellationTokenSource tokenSource = null)
        {
            ThreadingPromise<TResult> promise = Envirment.Current.GetModule<PoolModule>().Get<ThreadingPromise<TResult>, ThreadingPromise<TResult>.Poolable>();
            FTask<TResult> fTask = Envirment.Current.GetModule<PoolModule>().Get<FTask<TResult>, FTask<TResult>.Poolable>();
            promise.InvokeTask = task;
            promise.BindTask = fTask.GetAwaiter();
            promise.BindToken = tokenSource;
            fTask.m_TaskFlow = promise;
            return fTask;
        }

    
    }
}
