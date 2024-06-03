

using System.Collections.Generic;

namespace FFramework
{
    public class WindowManager : IModule
    {
        void IModule.OnCreate(object moduleParameter)
        {

        }

        void IModule.OnDestroy()
        {

        }

        private LinkedList<IPanel> m_OpeningPanelStack = new LinkedList<IPanel>();


    }
}
