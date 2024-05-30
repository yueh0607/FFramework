using System;
using System.Diagnostics;
using System.Threading;

namespace FFramework
{
    public class ThreadEnvirment : Envirment
    {
        private Thread m_Thread;
        private long m_DueDeltaMillseconds;
        private Stopwatch m_Stopwatch;
        private float m_DeltaTime;

        private FSynchronizationContext m_MailBox;
        public override FSynchronizationContext MailBox {get => m_MailBox; protected set => m_MailBox = value; }

        public ThreadEnvirment(long dueDeltaMillseconds)
        {
            m_MailBox = new FSynchronizationContext();

            m_DeltaTime = 0;
            m_Stopwatch = new Stopwatch();
            m_DueDeltaMillseconds = dueDeltaMillseconds;
            m_Thread = new Thread(ThreadMain);

            RegisterEnvirment(m_Thread);
            m_Thread.Start();
        }

        void ThreadMain()
        {
            EventModule eventModule = GetModule<EventModule>();
            while (true)
            {
                m_Stopwatch.Restart();
                eventModule.Publisher.SendAll<IUpdate>(m_DeltaTime);
                Thread.Sleep(Math.Clamp((int)(m_DueDeltaMillseconds - m_Stopwatch.ElapsedMilliseconds), 0, int.MaxValue));
                m_DeltaTime = m_Stopwatch.ElapsedMilliseconds / 1000f;
            }
        }
    }
}
