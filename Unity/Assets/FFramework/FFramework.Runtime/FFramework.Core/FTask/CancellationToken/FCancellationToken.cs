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
        internal IFTaskAwaiter Awaiter;


        public bool IsCancellationRequested => m_IsCancellationRequested;

        public bool IsSuspendRequested => m_IsSuspendRequested;



        internal void InternalRegisterCancelCallback(System.Action callback)
        {
            m_CancelCallback += callback;
        }

        internal void InternalRegisterSuspendCallback(params System.Action[] restoreCallback)
        {
            foreach(var callback in restoreCallback)
            {
                m_SuspendCallback += callback;
            }

        }

        internal void InternalCancel()
        {
            m_IsCancellationRequested = true;
            Awaiter?.SetCanceled();
            m_CancelCallback?.Invoke();
        }

        internal void InternalSuspend()
        {
            m_IsSuspendRequested = true;
            Awaiter?.SetSuspend();
        }

        internal void InternalRestore()
        {
            m_IsSuspendRequested = false;
            Awaiter?.SetRestore();
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

        private async FTask PrivateSuspendAfter(TimeSpan time)
        {
            await FTask.Delay(time);
            Suspend();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SuspendAfter(TimeSpan time)
        => PrivateSuspendAfter(time).Forget();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SuspendAfterSeconds(float seconds)
            => PrivateSuspendAfter(TimeSpan.FromSeconds(seconds)).Forget();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SuspendAfterMillseconds(int millseconds)
            => PrivateSuspendAfter(TimeSpan.FromMilliseconds(millseconds)).Forget();




        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Restore()
            => InternalRestore();

        private async FTask PrivateRestoreAfter(TimeSpan time)
        {
            await FTask.Delay(time);
            Restore();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RestoreAfter(TimeSpan time)
        => PrivateRestoreAfter(time).Forget();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RestoreAfterSeconds(float seconds)
            => PrivateRestoreAfter(TimeSpan.FromSeconds(seconds)).Forget();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RestoreAfterMillseconds(int millseconds)
            => PrivateRestoreAfter(TimeSpan.FromMilliseconds(millseconds)).Forget();






        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RegisterCancelCallback(System.Action callback)
            => InternalRegisterCancelCallback(callback);


        public static implicit operator bool(FCancellationToken token)
        {
            return token != null && token.IsCancellationRequested;
        }

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
                obj.Awaiter = null;
                obj.m_IsCancellationRequested = false;
                obj.m_IsSuspendRequested = false;
                obj.m_SuspendCallback = null;
            }
        }
    }
}
