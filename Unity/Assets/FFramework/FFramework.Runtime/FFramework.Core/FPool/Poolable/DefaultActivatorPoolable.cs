using System;

namespace FFramework
{
    public static partial class BuiltInPoolables
    {
        /// <summary>
        /// 默认Activator创建的Poolable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class DefaultActivatorPoolable<T> : IPoolable<T> where T : class
        {
            int IPoolable.Capacity => 1000;

            T IPoolable<T>.OnCreate()
            {
                return Activator.CreateInstance<T>();
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