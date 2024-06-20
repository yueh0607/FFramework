namespace FFramework
{
    public interface IComponent
    {
        IEntity Entity { get; }
    }


    public interface IComponent<T> : IComponent where T : ComponentBranch
    {
        new T Entity { get; }
    }
}
