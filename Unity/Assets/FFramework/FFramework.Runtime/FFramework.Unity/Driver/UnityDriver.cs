using System.Threading;
using UnityEngine;
namespace FFramework
{

    public class UnityDriver : MonoBehaviour
    {
        Envirment m_UnityEnvirment;
        EventModule m_EventModule;

        private void Awake()
        {
            m_UnityEnvirment = new UnityEnvirment();  
            m_EventModule = m_UnityEnvirment.GetModule<EventModule>();
        }


        private void Update()
        {
            m_EventModule.Publisher.SendAll<IUpdate>(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            m_EventModule.Publisher.SendAll<IFixedUpdate>(Time.fixedDeltaTime);
        }

        private void OnGUI()
        {
            m_EventModule.Publisher.SendAll<IOnGUI>();
        }


        Timer realTimer;
        private void Start()
        {
            realTimer = new Timer(OnRealTimeFlow, null, 0, 10);
        }

        private void OnRealTimeFlow(object state)
        {
            m_EventModule.Publisher.SendAll<IRealTimeUpdate>(0.01f);
        }

        private void OnDestroy()
        {
            realTimer.Dispose();
        }
    }
}
