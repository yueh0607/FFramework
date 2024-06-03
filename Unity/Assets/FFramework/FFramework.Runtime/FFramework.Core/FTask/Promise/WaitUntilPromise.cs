using System;

namespace FFramework
{
    internal class WaitUntilPromise : LifeCycleSwitch<WaitUntilPromise>, IFTaskFlow
    {

        public Func<bool> WaitCondition { get; set; }

        public ISucceedCallback BindTask { get; set; } = null;

        public bool WhileMode { get; set; } = false;

        void IFTaskFlow.OnCancel()
        {
            BindTask.SetSucceed();
            Envirment.Current.GetModule<PoolModule>().Set<WaitUntilPromise, WaitUntilPromise.Poolable>(this);
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
            Paused = false;
        }

        void IFTaskFlow.OnSucceed()
        {
            Envirment.Current.GetModule<PoolModule>().Set<WaitUntilPromise, WaitUntilPromise.Poolable>(this);
        }

        void IFTaskFlow.OnSuspend()
        {
            Paused = true;
        }


        protected override void OnTimerUpdate(float deltaTime)
        {
            if (!WhileMode)
            {
                if (WaitCondition())
                {
                    BindTask.SetSucceed();
                }
            }
            else      
            {
                if (!WaitCondition())
                    BindTask.SetSucceed();
            }
        }

        public new class Poolable : LifeCycleSwitch<WaitUntilPromise>.Poolable
        {

            public override void OnSet(WaitUntilPromise obj)
            {
                base.OnSet(obj);
                obj.WhileMode = false;
                obj.WaitCondition = null;
            }
        }
    }
}
