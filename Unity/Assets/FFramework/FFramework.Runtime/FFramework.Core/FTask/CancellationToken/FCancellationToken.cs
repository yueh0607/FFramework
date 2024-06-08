using System;
using System.Runtime.CompilerServices;

namespace FFramework
{
    public class FCancellationToken : FUnit
    {

        private bool m_IsCancellationRequested;


        private bool m_IsSuspendRequested;


        private System.Action m_CancelCallback;

        private System.Action m_SuspendCallback;
        internal IFTaskFlow Flow { get; set; }


        public bool IsCancellationRequested => m_IsCancellationRequested;

        public bool IsSuspendRequested => m_IsSuspendRequested;



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
            Flow?.OnCancel();
            m_CancelCallback?.Invoke();
        }

        internal void InternalSuspend()
        {
            m_IsSuspendRequested = true;
            Flow?.OnSuspend();
        }

        internal void InternalRestore()
        {
            m_IsSuspendRequested = false;
            Flow?.OnRestore();
            m_SuspendCallback?.Invoke();
        }



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Cancel()
            => InternalCancel();

        private async FTask PrivateCancelAfter(TimeSpan time)
        {
            await FTask.Delay(time);
            Cancel();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CancelAfter(TimeSpan time)
            => PrivateCancelAfter(time).Forget();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CancelAfterSeconds(float seconds)
            => PrivateCancelAfter(TimeSpan.FromSeconds(seconds)).Forget();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CancelAfterMillseconds(int millseconds)
            => PrivateCancelAfter(TimeSpan.FromMilliseconds(millseconds)).Forget();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Suspend()
            => InternalSuspend();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Restore()
            => InternalRestore();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RegisterCancelCallback(System.Action callback)
            => InternalRegisterCancelCallback(callback);


        public class Poolable : IPoolable<FCancellationToken>
        {
            int IPoolable.Capacity => FTaskConst.NOT_GENERIC_FTASK_PER_TYPE_POOL_CAPACITY;

            FCancellationToken IPoolable<FCancellationToken>.OnCreate()
            {
                return new FCancellationToken();
            }

            void IPoolable<FCancellationToken>.OnDestroy(FCancellationToken obj)
            {

            }

            void IPoolable<FCancellationToken>.OnGet(FCancellationToken obj)
            {

            }

            void IPoolable<FCancellationToken>.OnSet(FCancellationToken obj)
            {
                obj.Flow = null;
                obj.m_IsCancellationRequested = false;
                obj.m_IsSuspendRequested = false;
                obj.m_SuspendCallback = null;
            }
        }
    }
}
