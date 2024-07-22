using System;
using System.Collections.Generic;
using System.Reflection;

namespace FFramework
{
    public class Scope : FUnit, IScope
    {
        private Dictionary<Type, object> m_Container = new Dictionary<Type, object>();

        internal void InternalRegister(Type type, object obj, bool allowOverride = false)
        {
            if (m_Container.ContainsKey(type))
            {
                if (allowOverride)
                    m_Container[type] = obj;
                else throw new InvalidOperationException("Object of type " + type.Name + " already exists in the container");
            }
            else
                m_Container.Add(type, obj);
        }

        public void Register<T>(T obj)
            => InternalRegister(typeof(T), obj);


        internal object InternalResolve(Type type)
        {
            if (m_Container.TryGetValue(type, out object obj))
                return obj;
            throw new NullReferenceException("No object of type " + type.Name + " found in the container");
        }

        public T Resolve<T>()
            => (T)InternalResolve(typeof(T));


        /// <summary>
        /// 【高耗时API】
        /// </summary>
        public void Inject(object target)
        {
            var type = target.GetType();
            var members = type.GetMembers();
            for(int i=0;i<members.Length; i++)
            {
                MemberInfo mi = members[i];
                if(mi.GetCustomAttribute<InjectAttribute>()!=null)
                {
                    if(mi is FieldInfo fi)
                    {
                        fi.SetValue(target, InternalResolve(fi.FieldType));
                    }
                    else if(mi is PropertyInfo pi && pi.CanWrite)
                    {
                        pi.SetValue(target, InternalResolve(pi.PropertyType));
                    }
                }
            }
        }

        public static Scope Global => SingletonProperty<Scope>.Instance;
    }
}
