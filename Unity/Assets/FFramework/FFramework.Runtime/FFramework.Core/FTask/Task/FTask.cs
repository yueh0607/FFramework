using System.Runtime.CompilerServices;

namespace FFramework
{
    [AsyncMethodBuilder(typeof(FTaskAsyncMethodBuilder))]
    public partial class FTask : FUnit, IFTask,IAsyncMachineCurrent

    {
        private FTaskAwaiter m_Awaiter;

        public FTaskAwaiter GetAwaiter() => m_Awaiter;

        internal IFTaskFlow m_TaskFlow = null;

        IFTaskFlow IFTask.Flow => m_TaskFlow;

        //可能被使用（表示当前MethodBuilder所等待的Awaiter）,也可能是null
        private IFTaskAwaiter m_CurrentAwaiter { get; set; }
        IFTaskAwaiter IAsyncMachineCurrent.CurrentAwaiter
        {
            get=> m_CurrentAwaiter;
            set => m_CurrentAwaiter = value;
        }
        internal void SetFlow(IFTaskFlow flow)
        {
            m_TaskFlow = flow;
        }

        //防止外部实例化，只允许通过池创建
        private FTask() { }


        public class Poolable : IPoolable<FTask>
        {
            int IPoolable.Capacity => FTaskConst.NOT_GENERIC_FTASK_PER_TYPE_POOL_CAPACITY;

            FTask IPoolable<FTask>.OnCreate()
            {
                FTask m_FTask = new FTask();
                return m_FTask;
            }

            void IPoolable<FTask>.OnDestroy(FTask obj)
            {
                //do nothing
            }

            void IPoolable<FTask>.OnGet(FTask obj)
            {
                obj.m_Awaiter = Envirment.Current.GetModule<PoolModule>().Get<FTaskAwaiter, FTaskAwaiter.Poolable>();
                obj.m_Awaiter.BindTask = obj;
            }

            void IPoolable<FTask>.OnSet(FTask obj)
            {
                Envirment.Current.GetModule<PoolModule>().Set<FTaskAwaiter, FTaskAwaiter.Poolable>(obj.m_Awaiter);
                obj.m_Awaiter = null;
                obj.m_TaskFlow = null;
            }
        }
    }


    [AsyncMethodBuilder(typeof(FTaskAsyncMethodBuilder<>))]
    public partial class FTask<T> : FUnit, IFTask
    {
        private FTaskAwaiter<T> m_Awaiter;

        public FTaskAwaiter<T> GetAwaiter() => m_Awaiter;

        internal IFTaskFlow m_TaskFlow = null;

        IFTaskFlow IFTask.Flow => m_TaskFlow;

        private FTask() { }

        public class Poolable : IPoolable<FTask<T>>
        {
            int IPoolable.Capacity => FTaskConst.NOT_GENERIC_FTASK_PER_TYPE_POOL_CAPACITY;

            FTask<T> IPoolable<FTask<T>>.OnCreate()
            {
                FTask<T> m_FTask = new FTask<T>();
                return m_FTask;
            }

            void IPoolable<FTask<T>>.OnDestroy(FTask<T> obj)
            {
                //do nothing
            }

            void IPoolable<FTask<T>>.OnGet(FTask<T> obj)
            {
                obj.m_Awaiter = Envirment.Current.GetModule<PoolModule>()
                    .Get<FTaskAwaiter<T>, FTaskAwaiter<T>.Poolable>();
                obj.m_Awaiter.BindTask = obj;
            }

            void IPoolable<FTask<T>>.OnSet(FTask<T> obj)
            {

                Envirment.Current.GetModule<PoolModule>()
                    .Set<FTaskAwaiter<T>, FTaskAwaiter<T>.Poolable>(obj.m_Awaiter);
                obj.m_Awaiter = null;
                obj.m_TaskFlow = null;
            }
        }
    }
}