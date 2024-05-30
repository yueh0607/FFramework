namespace FFramework
{
    [ModuleStatic("FEvent")]
    public class EventModule : IModule
    {
        public IEventPublisher Publisher { get; private set; }
        void IModule.OnCreate(object moduleParameter)
        {
            Publisher = new FEventPublisher();
        }

        void IModule.OnDestroy()
        {
            Publisher = null;
        }
    }
}
