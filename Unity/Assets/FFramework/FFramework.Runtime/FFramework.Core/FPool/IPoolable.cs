namespace FFramework
{
    public interface IPoolable
    {
        int Capacity { get; }

    }

    public interface IPoolable<T> : IPoolable
    {

        T OnCreate();

        void OnGet(T obj);

        void OnSet(T obj);

        void OnDestroy(T obj);
    }
}

