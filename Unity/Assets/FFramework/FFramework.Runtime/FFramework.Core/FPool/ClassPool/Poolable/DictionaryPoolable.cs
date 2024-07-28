using System.Collections;

namespace FFramework
{
    public class DictionaryPoolable<T> : IPoolable<T> where T : IDictionary, new()
    {
        int IPoolable.Capacity => 1000;

        T IPoolable<T>.OnCreate()
        {
            return new T();
        }

        void IPoolable<T>.OnDestroy(T obj)
        {

        }

        void IPoolable<T>.OnGet(T obj)
        {

        }

        void IPoolable<T>.OnSet(T obj)
        {
            obj.Clear();
        }
    }
}
