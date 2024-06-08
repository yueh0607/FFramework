namespace FFramework
{
    internal class DelayPromise : LifeCycleSwitch<DelayPromise>, IFTaskFlow
    {
        public float CurrentTime { get; private set; } = 0;
        public float TargetTime { get; internal set; } = 0;
        public ISucceedCallback BindTask { get; set; } = null;

        void IFTaskFlow.OnCancel()
        {
            BindTask.SetSucceed();
            Envirment.Current.GetModule<PoolModule>().Set<DelayPromise, DelayPromise.Poolable>(this);
        }

        void IFTaskFlow.OnFailed()
        {
            // on expection happend
        }

        void IFTaskFlow.OnRestore()
        {
            Paused = false;
        }

        void IFTaskFlow.OnStart()
        {
            Paused = false;
        }

        void IFTaskFlow.OnSucceed()
        {
            Envirment.Current.GetModule<PoolModule>().Set<DelayPromise, DelayPromise.Poolable>(this);
        }

        void IFTaskFlow.OnSuspend()
        {
            Paused = true;
        }

        protected override void OnTimerUpdate(float deltaTime)
        {
            CurrentTime += deltaTime;
            if (CurrentTime >= TargetTime)
            {
                BindTask.SetSucceed();
            }
        }

        public new class Poolable : LifeCycleSwitch<DelayPromise>.Poolable
        {
            public override void OnSet(DelayPromise obj)
            {
                base.OnSet(obj);
                obj.CurrentTime = 0;
                obj.TargetTime = 0;
                obj.BindTask = null;
            }
        }
    }
}
