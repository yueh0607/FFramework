using System.Runtime.CompilerServices;

namespace FFramework
{
    public struct FCancellationTokenProxy
    {

        private bool m_IsCancellationRequested;

        public readonly bool IsCancellationRequested => m_IsCancellationRequested;

        private bool m_IsSuspendRequested;

        public readonly bool IsSuspendRequested => m_IsSuspendRequested;

        private System.Action m_CancelCallback;

        private System.Action m_SuspendCallback;



        internal void InternalRegisterCancelCallback(System.Action callback)
        {
            m_CancelCallback += callback;
        }

        internal void InternalRegisterSuspendCallback(System.Action callback, System.Action restoreCallback)
        {
            m_SuspendCallback += restoreCallback;
            m_SuspendCallback += callback;
        }

        internal void InternalCancel()
        {
            m_IsCancellationRequested = true;
            m_CancelCallback?.Invoke();
        }


        internal void InternalSuspend()
        {
            m_IsSuspendRequested = true;
        }

        internal void InternalRestore()
        {
            m_IsSuspendRequested = false;
            m_SuspendCallback?.Invoke();
        }
    }
}