namespace FFramework
{

    public partial class FSwitchThreadTask : FUnit, IFTask
    {
        private FSwitchThreadAwaiter m_Awaiter;

        public FSwitchThreadAwaiter GetAwaiter() => m_Awaiter;

        internal IFTaskFlow m_TaskFlow = null;

        IFTaskFlow IFTask.Flow => m_TaskFlow;

        internal FSynchronizationContext SwitchConext { get; set; }

        //可能被使用（表示当前MethodBuilder所等待的Awaiter）,也可能是null
        internal IFTaskAwaiter CurrentAwaiter { get; set; }

        internal void SetFlow(IFTaskFlow flow)
        {
            m_TaskFlow = flow;
        }

        //防止外部实例化，只允许通过池创建
        private FSwitchThreadTask() { }


        public class Poolable : IPoolable<FSwitchThreadTask>
        {
            int IPoolable.Capacity => FTaskConst.NOT_GENERIC_FTASK_PER_TYPE_POOL_CAPACITY;

            FSwitchThreadTask IPoolable<FSwitchThreadTask>.OnCreate()
            {
                FSwitchThreadTask m_FTask = new FSwitchThreadTask();
                return m_FTask;
            }

            void IPoolable<FSwitchThreadTask>.OnDestroy(FSwitchThreadTask obj)
            {
                //do nothing
            }

            void IPoolable<FSwitchThreadTask>.OnGet(FSwitchThreadTask obj)
            {
                obj.m_Awaiter = Envirment.Current.GetModule<PoolModule>().Get<FSwitchThreadAwaiter, FSwitchThreadAwaiter.Poolable>();
                obj.m_Awaiter.BindTask = obj;
            }

            void IPoolable<FSwitchThreadTask>.OnSet(FSwitchThreadTask obj)
            {
                Envirment.Current.GetModule<PoolModule>().Set<FSwitchThreadAwaiter, FSwitchThreadAwaiter.Poolable>(obj.m_Awaiter);
                obj.m_Awaiter = null;
                obj.m_TaskFlow = null;
            }
        }
    }
}
