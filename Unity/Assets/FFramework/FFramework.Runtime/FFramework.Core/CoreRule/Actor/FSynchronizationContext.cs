using System.Collections.Concurrent;
using System.Threading;

namespace FFramework
{
    public class FSynchronizationContext : SynchronizationContext, IUpdate
    {
        private ConcurrentQueue<IMessagePack> m_TaskQueue = new ConcurrentQueue<IMessagePack>();

        public Envirment BelongEnvirment { get; internal set; }

        public override void Post(SendOrPostCallback d, object state)
        {
            SendOrPostCallbackMessagePack pack = BelongEnvirment.GetModule<PoolModule>()
                .Get<SendOrPostCallbackMessagePack, SendOrPostCallbackMessagePack.Poolable>();

            pack.Callback = d;
            pack.State = state;

            Post(pack);
        }

        public void Post(IMessagePack message)
        {
            m_TaskQueue.Enqueue(message);
        }

        void IUpdate.Update(float deltaTime)
        {
            if (m_TaskQueue.TryDequeue(out IMessagePack message))
            {
                OnHandleMessgae(message);

                var pool = BelongEnvirment.GetModule<PoolModule>();
                pool.InternalSet(message.GetType(),message);
            }
            else
            {
                //防止持续自旋占用资源
                Thread.Sleep(10);
            }
        }

        protected virtual void OnHandleMessgae(IMessagePack message)
        {
            message.Invoke();
        }
    }
}
