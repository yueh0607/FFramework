using System.Threading;

namespace FFramework
{
    public class SendOrPostCallbackMessagePack : IMessagePack
    {
        public SendOrPostCallback Callback { get; set; }
        public object State { get; set; } = null;

        public void Invoke()
        {
            Callback?.Invoke(State);
        }

        public class Poolable : IPoolable<SendOrPostCallbackMessagePack>
        {
            int IPoolable.Capacity => 1000;

            SendOrPostCallbackMessagePack IPoolable<SendOrPostCallbackMessagePack>.OnCreate()
            {
                return new SendOrPostCallbackMessagePack();
            }

            void IPoolable<SendOrPostCallbackMessagePack>.OnDestroy(SendOrPostCallbackMessagePack obj)
            {
                
            }

            void IPoolable<SendOrPostCallbackMessagePack>.OnGet(SendOrPostCallbackMessagePack obj)
            {
                
            }

            void IPoolable<SendOrPostCallbackMessagePack>.OnSet(SendOrPostCallbackMessagePack obj)
            {
                obj.Callback = null;
                obj.State = null;
            }
        }

    }
}
