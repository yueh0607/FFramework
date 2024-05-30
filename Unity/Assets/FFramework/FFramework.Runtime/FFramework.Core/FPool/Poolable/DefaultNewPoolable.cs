namespace FFramework
{
    public static partial class BuiltInPoolables
    {
        public class DefaultNewPoolable<T> : IPoolable<T> where T : class, new()
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

            }
        }
    }
}