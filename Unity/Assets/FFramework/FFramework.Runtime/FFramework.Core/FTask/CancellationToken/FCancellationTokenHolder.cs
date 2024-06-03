using System;
using System.Runtime.CompilerServices;

namespace FFramework
{
    public class FCancellationTokenHolder : FUnit
    {
        private FCancellationToken m_token = default;

        internal FCancellationToken Token => m_token;


        internal void SetToken(FCancellationToken token) => m_token = token;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Cancel()
            => m_token.InternalCancel();

        private async FTask PrivateCancelAfter(TimeSpan time)
        {
            await FTask.Delay(time);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CancelAfter(TimeSpan time)
            => PrivateCancelAfter(time).Forget();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CancelAfterSeconds(float seconds)
            => PrivateCancelAfter(TimeSpan.FromSeconds(seconds)).Forget();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CancelAfterMillseconds(int millseconds)
            => PrivateCancelAfter(TimeSpan.FromSeconds(millseconds)).Forget();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Suspend()
            => m_token.InternalSuspend();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Restore()
            => m_token.InternalRestore();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RegisterCancelCallback(System.Action callback)
            => m_token.InternalRegisterCancelCallback(callback);


        public class Poolable : IPoolable<FCancellationTokenHolder>
        {
            int IPoolable.Capacity => FTaskConst.NOT_GENERIC_FTASK_PER_TYPE_POOL_CAPACITY;

            FCancellationTokenHolder IPoolable<FCancellationTokenHolder>.OnCreate()
            {
                return new FCancellationTokenHolder();
            }

            void IPoolable<FCancellationTokenHolder>.OnDestroy(FCancellationTokenHolder obj)
            {

            }

            void IPoolable<FCancellationTokenHolder>.OnGet(FCancellationTokenHolder obj)
            {

            }

            void IPoolable<FCancellationTokenHolder>.OnSet(FCancellationTokenHolder obj)
            {
                obj.m_token = default;
            }
        }
    }
}
