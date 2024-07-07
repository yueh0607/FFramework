using System.Threading;

namespace FFramework
{
    public class FThreadQueueSynchronizationContext : FSynchronizationContext
    {
        protected override void OnHandleMessgae(IMessagePack message)
        {
            ThreadPool.QueueUserWorkItem((state) => message.Invoke());
        }
    }
}
