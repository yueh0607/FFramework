using System;
using System.Collections.Generic;

namespace FFramework
{
    public class FinalStateMachine : FDispoableUnit
    {
        public IState CurrentState { get; private set; } = null;

        private Dictionary<Type,IState> m_CreatedState = new Dictionary<Type,IState>();

        internal void InternalExitCurrentState()
        {
            CurrentState?.OnExit();
        }
        internal void InternalChangeState(Type type)
        {
            InternalExitCurrentState();

            if (m_CreatedState.ContainsKey(type))
            {
                CurrentState = m_CreatedState[type];
            }
            else
            {
                CurrentState = (IState)System.Activator.CreateInstance(type);
                m_CreatedState.Add(type,CurrentState);
                CurrentState.OnCreate();
            }

            CurrentState.OnEnter();
        }


        public void ChangeState<T>() where T : IState
            => InternalChangeState(typeof(T));

        public void ExitCurrentState()
            => InternalExitCurrentState();

        protected override void OnReleaseManagedResource()
        {
            InternalExitCurrentState();
            foreach (var state in m_CreatedState.Values)
            {
                state.OnDestroy();
            }
            m_CreatedState.Clear();
            m_CreatedState = null;
        }

        protected override void OnReleaseUnmanagedResource()
        {
            
        }
    }
}
