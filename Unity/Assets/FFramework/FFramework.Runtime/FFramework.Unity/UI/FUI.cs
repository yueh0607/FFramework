namespace FFramework
{
    [ModuleStatic("FUI")]
    public class FUIModule : IModule
    {
        void IModule.OnCreate(object moduleParameter)
        {

        }

        void IModule.OnDestroy()
        {

        }



        public void Load<T>() where T : IPanel
        {

        }

        public void Show<T>() where T : IPanel
        {

        }

        public void Show<T, K>(K panelParameters) where T : IPanel where K : struct
        {

        }

        public void Hide<T>() where T : IPanel
        {

        }

        public void Unload<T>() where T : IPanel
        {

        }
    }
}
