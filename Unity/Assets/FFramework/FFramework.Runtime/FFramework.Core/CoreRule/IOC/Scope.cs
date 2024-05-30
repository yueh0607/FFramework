using System;
using System.Collections.Generic;

namespace FFramework
{
    public class Scope : FUnit, IScope
    {
        private Dictionary<Type, object> m_Container = new Dictionary<Type, object>();

        internal void InternalRegister(Type type, object obj)
        {
            if (m_Container.ContainsKey(type))
                m_Container[type] = obj;
            else
                m_Container.Add(type, obj);
        }

        public void Register<T>(T obj)
            => InternalRegister(typeof(T), obj);


        internal T InternalResolve<T>()
        {
            if (m_Container.TryGetValue(typeof(T), out object obj))
                return (T)obj;
            throw new NullReferenceException("No object of type " + typeof(T).Name + " found in the container");
        }

        public T Resolve<T>()
            => InternalResolve<T>();


        public static Scope Global => SingletonProperty<Scope>.Instance;
    }
}
