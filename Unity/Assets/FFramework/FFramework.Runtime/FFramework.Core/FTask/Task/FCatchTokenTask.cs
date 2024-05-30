namespace FFramework
{
    public class FCatchTokenTask : FUnit, IFTask
    {
        private FCatchTokenTaskAwaiter m_Awaiter;

        public FCatchTokenTaskAwaiter GetAwaiter() => m_Awaiter;

        private IFTaskFlow m_TaskFlow = null;

        IFTaskFlow IFTask.Flow => m_TaskFlow;

        private FCatchTokenTask() { }

        public class Poolable : IPoolable<FCatchTokenTask>
        {
            int IPoolable.Capacity => FTaskConst.TOKEN_CATCH_POOL_CAPACITY;

            FCatchTokenTask IPoolable<FCatchTokenTask>.OnCreate()
            {
                FCatchTokenTask m_FCatchTokenTask = new FCatchTokenTask();
                return m_FCatchTokenTask;
            }

            void IPoolable<FCatchTokenTask>.OnDestroy(FCatchTokenTask obj)
            {
                //do nothing
            }

            void IPoolable<FCatchTokenTask>.OnGet(FCatchTokenTask obj)
            {
                obj.m_Awaiter = Envirment.Current.GetModule<PoolModule>().Get<FCatchTokenTaskAwaiter, FCatchTokenTaskAwaiter.Poolable>();
                obj.m_Awaiter.BindTask = obj;
            }

            void IPoolable<FCatchTokenTask>.OnSet(FCatchTokenTask obj)
            {
                Envirment.Current.GetModule<PoolModule>().Set<FCatchTokenTaskAwaiter, FCatchTokenTaskAwaiter.Poolable>(obj.m_Awaiter);
                obj.m_Awaiter = null;
                obj.m_TaskFlow = null;
            }
        }

    }
}
