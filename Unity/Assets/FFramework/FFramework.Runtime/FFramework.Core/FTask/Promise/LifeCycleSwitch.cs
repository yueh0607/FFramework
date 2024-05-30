using System;

namespace FFramework
{
    public enum ETimerLoop
    {
        Update,
        FixedUpdate,
        RealTimeUpdate
    }

    public abstract class LifeCycleSwitch<T> :
        IFixedUpdate, IUpdate, IRealTimeUpdate
        where T : LifeCycleSwitch<T>, new()
    {
        public ETimerLoop TimerLoop { get; set; } = ETimerLoop.Update;


        void IFixedUpdate.FixedUpdate(float deltaTime)
        {
            OnTimerUpdate(deltaTime);
        }

        void IUpdate.Update(float deltaTime)
        {
            OnTimerUpdate(deltaTime);
        }

        private bool m_Paused = true;

        public bool Paused
        {
            get => m_Paused;
            set
            {
                if (value)
                {
                    UnSubscribe();
                }
                else
                {
                    Subscribe();
                }
            }
        }

        protected void Subscribe()
        {
            switch (TimerLoop)
            {
                case ETimerLoop.Update:
                    Envirment.Current.GetModule<EventModule>()
                        .Publisher.Subscribe<IUpdate>(this);
                    return;
                case ETimerLoop.FixedUpdate:
                    Envirment.Current.GetModule<EventModule>()
                        .Publisher.Subscribe<IFixedUpdate>(this);
                    return;
                case ETimerLoop.RealTimeUpdate:
                    Envirment.Current.GetModule<EventModule>()
                        .Publisher.Subscribe<IRealTimeUpdate>(this);
                    return;
            }
            throw new InvalidOperationException("TimerLoop is not valid");
        }

        protected void UnSubscribe()
        {
            switch (TimerLoop)
            {
                case ETimerLoop.Update:
                    Envirment.Current.GetModule<EventModule>()
                        .Publisher.UnSubscribe<IUpdate>(this);
                    return;
                case ETimerLoop.FixedUpdate:
                    Envirment.Current.GetModule<EventModule>()
                        .Publisher.UnSubscribe<IFixedUpdate>(this);
                    return;
                case ETimerLoop.RealTimeUpdate:
                    Envirment.Current.GetModule<EventModule>()
                        .Publisher.UnSubscribe<IRealTimeUpdate>(this);
                    return;
            }
            throw new InvalidOperationException("TimerLoop is not valid");
        }


        protected abstract void OnTimerUpdate(float deltaTime);

        void IRealTimeUpdate.RealUpdate(float deltaTime)
        {
            OnTimerUpdate(deltaTime);
        }

        public abstract class Poolable : IPoolable<T>
        {
            public int Capacity => 1000;

            public virtual T OnCreate() => new T();

            public virtual void OnDestroy(T obj) { }

            public virtual void OnGet(T obj) { }

            public virtual void OnSet(T obj)
            {
                obj.Paused = true;
                obj.TimerLoop = ETimerLoop.Update;
            }
        }
    }
}
