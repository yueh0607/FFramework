using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace FFramework
{
    public partial class FTask
    {
        public static FCatchTokenTask CompletedTask => Envirment.Current.GetModule<PoolModule>().Get<FCatchTokenTask, FCatchTokenTask.Poolable>();


        public static FTask DelayMillseconds(int millseconds, ETimerLoop loop = ETimerLoop.Update)
            => Delay(TimeSpan.FromMilliseconds(millseconds));

        public static FTask DelaySeconds(float seconds, ETimerLoop loop = ETimerLoop.Update)
            => Delay(TimeSpan.FromSeconds(seconds), loop);


        public static FTask Delay(TimeSpan delayTime, ETimerLoop loop = ETimerLoop.Update)
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

            FCancellationToken tokenHolder = await FTask.CatchToken();
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

            FCancellationToken tokenHolder = await FTask.CatchToken();
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
            return Envirment.Current.GetModule<PoolModule>()
                .Get<FCatchTokenTask, FCatchTokenTask.Poolable>();
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


         static async FTask DelayLoop(TimeSpan delayTime,Action<TimeSpan> callback, ETimerLoop loop)
        {
            var m_Token = await FTask.CatchToken();
            TimeSpan lastSpan = TimeSpan.Zero;
            while (!m_Token.IsCancellationRequested)
            {
                await FTask.Delay(delayTime, loop: loop);

                if (m_Token.IsCancellationRequested)
                    break;

                lastSpan += delayTime;
                callback(lastSpan);
            }
        }
        public static FCancellationToken Tick(TimeSpan time,Action<TimeSpan> callback,ETimerLoop loop = ETimerLoop.Update)
        {
            FCancellationToken token = 
                Envirment.Current.GetModule<PoolModule>().Get<FCancellationToken, FCancellationToken.Poolable>();


            DelayLoop(time,callback,loop).Forget(token);
            return token;
        }
    }



    public static class FTaskExtensions
    {

        /// <summary>
        /// 使用某个令牌阻挡本状态机下令牌的传播
        /// </summary>
        /// <param name="fTask"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static FTask BlockToken(this FTask fTask, FCancellationToken token)
        {
            ((IFTaskAwaiter)fTask.GetAwaiter()).SetToken(token);
            return fTask;
        }

        /// <summary>
        /// 使用某个令牌阻挡本状态机下令牌的传播
        /// </summary>
        /// <param name="fTask"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static FTask<T> BlockToken<T>(this FTask<T> fTask, FCancellationToken token)
        {
            ((IFTaskAwaiter)fTask.GetAwaiter()).SetToken(token);
            return fTask;
        }


        /// <summary>
        /// 绑定令牌后遗忘此任务
        /// </summary>
        /// <param name="task"></param>
        /// <param name="source"></param>
        public static void Forget(this FTask task, FCancellationToken source)
        {
            ((IFTaskAwaiter)(task.GetAwaiter())).SetToken(source);
            Forget(task);
        }
        /// <summary>
        /// 遗忘此任务
        /// </summary>
        /// <param name="task"></param>
        /// <param name="source"></param>
        public static void Forget(this FTask task)
        {
            task.GetAwaiter().SetSyncSucceed();
        }

        /// <summary>
        /// 绑定令牌后遗忘此任务
        /// </summary>
        /// <param name="task"></param>
        /// <param name="source"></param>
        public static void Forget<T>(this FTask<T> task, FCancellationToken tokenHolder)
        {
            ((IFTaskAwaiter)(task.GetAwaiter())).SetToken(tokenHolder);
            Forget<T>(task);
        }

        /// <summary>
        /// 遗忘此任务
        /// </summary>
        /// <param name="task"></param>
        /// <param name="source"></param>
        public static void Forget<T>(this FTask<T> task)
        {
            task.GetAwaiter().SetSyncSucceed();
        }

        /// <summary>
        /// 将Task转为FTask（将会丢失Suspend功能）
        /// </summary>
        /// <param name="task"></param>
        /// <param name="tokenSource"></param>
        /// <returns></returns>
        public static FTask ToFTask(this Task task, CancellationTokenSource tokenSource = null)
        {
            ThreadingPromise promise = Envirment.Current.GetModule<PoolModule>().Get<ThreadingPromise, ThreadingPromise.Poolable>();
            FTask fTask = Envirment.Current.GetModule<PoolModule>().Get<FTask, FTask.Poolable>();
            promise.InvokeTask = task;
            promise.BindTask = fTask.GetAwaiter();
            promise.BindToken = tokenSource;
            promise.ExceptionCallback = fTask.GetAwaiter();
            fTask.m_TaskFlow = promise;
            return fTask;
        }

        /// <summary>
        /// 将Task转为FTask（将会丢失Suspend功能）
        /// </summary>
        /// <param name="task"></param>
        /// <param name="tokenSource"></param>
        /// <returns></returns>
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
