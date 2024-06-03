namespace FFramework
{
    internal class WaitCountPromise : FUnit, IFTaskFlow
    {
        public ISucceedCallback BindTask;
        public int WaitCount { get; set; } = 1;

        public FTask[] WaitArray { get; set; }

        private bool m_Paused = false;

        public bool Paused
        {
            get => m_Paused;
            set
            {
                if (m_Paused && !value && m_CompletedCount >= WaitCount)
                {
                    BindTask.SetSucceed();
                }
                m_Paused = value;
            }
        }

        private int m_CompletedCount = 0;
        public int CompletedCount
        {
            get => m_CompletedCount;
            private set
            {
                m_CompletedCount = value;
                if (!Paused && m_CompletedCount >= WaitCount)
                {
                    BindTask.SetSucceed();
                }
            }
        }

        public FCancellationTokenHolder TokenHolder { get; set; }

        public class WaitCountAddFlow : IFTaskFlow
        {
            private WaitCountPromise m_Promise;
            void IFTaskFlow.OnCancel()
            {

            }

            void IFTaskFlow.OnFailed()
            {

            }

            void IFTaskFlow.OnRestore()
            {

            }

            void IFTaskFlow.OnStart()
            {

            }

            void IFTaskFlow.OnSucceed()
            {
                m_Promise.CompletedCount++;
            }

            void IFTaskFlow.OnSuspend()
            {

            }
        }

        private WaitCountAddFlow m_WaitAddFlow = new WaitCountAddFlow();


        void IFTaskFlow.OnCancel()
        {
            BindTask.SetSucceed();
            Envirment.Current.GetModule<PoolModule>().Set<WaitCountPromise, WaitCountPromise.Poolable>(this);
        }

        void IFTaskFlow.OnFailed()
        {

        }

        void IFTaskFlow.OnRestore()
        {
            Paused = false;
        }

        void IFTaskFlow.OnStart()
        {
            foreach (var task in WaitArray)
            {
                task.SetFlow(m_WaitAddFlow);
                task.Forget(TokenHolder);
            }
        }

        void IFTaskFlow.OnSucceed()
        {

        }

        void IFTaskFlow.OnSuspend()
        {
            Paused = true;
        }

        public class Poolable : IPoolable<WaitCountPromise>
        {
            int IPoolable.Capacity => 1000;

            WaitCountPromise IPoolable<WaitCountPromise>.OnCreate()
            {
                return new WaitCountPromise();
            }

            void IPoolable<WaitCountPromise>.OnDestroy(WaitCountPromise obj)
            {

            }

            void IPoolable<WaitCountPromise>.OnGet(WaitCountPromise obj)
            {

            }

            void IPoolable<WaitCountPromise>.OnSet(WaitCountPromise obj)
            {
                obj.BindTask = null;
                obj.WaitCount = 1;
                obj.WaitArray = null;
                obj.TokenHolder = null;
                obj.m_Paused = false;
                obj.m_CompletedCount = 0;
            }
        }
    }
}
