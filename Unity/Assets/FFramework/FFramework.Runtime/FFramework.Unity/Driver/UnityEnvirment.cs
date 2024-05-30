using System.Threading;

namespace FFramework
{
    public class UnityEnvirment : Envirment
    {
        private FSynchronizationContext m_MailBox = new FSynchronizationContext();
        public override FSynchronizationContext MailBox { get => m_MailBox;protected set => m_MailBox = value; }
        
        public UnityEnvirment()
        {
            //注册主线程
            RegisterEnvirment(Thread.CurrentThread);
        }
    }
}
