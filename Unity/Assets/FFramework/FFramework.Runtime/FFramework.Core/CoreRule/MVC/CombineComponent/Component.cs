namespace FFramework
{
    public class Component : IComponent
    {
        public IEntity Entity { get; internal set; }


    }


    public class Component<T> : Component, IComponent<T> where T : ComponentBranch
    {
        public new T Entity { get; internal set; }

    }
}
