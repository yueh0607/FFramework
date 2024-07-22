using System;
using System.Collections.Generic;

namespace FFramework
{
    public class ComponentBranch
    {
        private Dictionary<Type,Entity> m_Components;

        public ComponentBranch()
        {
            m_Components = new Dictionary<Type, Entity>();
        }
        public int ComponentCount => m_Components.Count;

        public void Add<ComponentType>() where ComponentType : Entity
        {
            var type = typeof(ComponentType);
            if (!m_Components.ContainsKey(type))
            {
                ComponentType component = Activator.CreateInstance<ComponentType>();
                m_Components.Add(type, component);
                component.Send<IEntityAdd>();
            }
        }

        public void Add(Type type)
        {
            CheckType(type);

            if (!m_Components.ContainsKey(type))
            {
                Entity component = (Entity)Activator.CreateInstance(type);
                m_Components.Add(type, component);
                component.Send<IEntityAdd>();
            }
        }
        public void Add(Entity component)
        {
            var type = component.GetType();
            if (!m_Components.ContainsKey(type))
            {
                m_Components.Add(type, component);
                component.Send<IEntityAdd>();
            }
        }

        public bool Remove(Entity component)
        {
            var type = component.GetType();
            if (!m_Components.ContainsKey(type))
            {
                component.Send<IEntityRemove>();
                m_Components.Remove(type);
                return true;
            }
            return false;
        }

        public bool Remove(Type type)
        {
            CheckType(type);

            if (!m_Components.ContainsKey(type))
            {
                var component = m_Components[type];
                component.Send<IEntityRemove>();
                m_Components.Remove(type);
                return true;
            }
            return false;
        }
        public bool Remove<ComponentType>() where ComponentType : Entity
        {
            var type = typeof(ComponentType);
            if (!m_Components.ContainsKey(type))
            {
                var component = m_Components[type];
                component.Send<IEntityRemove>();
                m_Components.Remove(type);
                return true;
            }
            return false;
        }


        public Entity GetComponent(Type type)
        {
            if (CheckType(type, throwOnError: false)) return null;

            if (m_Components.TryGetValue(type, out var component))
                return component;

            return null;
        }

        public Entity GetComponent<ComponentType>() where ComponentType : Entity
        {
            var type = typeof(ComponentType);
            if (m_Components.TryGetValue(type, out var component))
                return component;

            return null;
        }



        bool CheckType(Type type, bool throwOnError = true)
        {
            if (!typeof(Entity).IsAssignableFrom(type))
            {
                if (throwOnError)
                    throw new InvalidOperationException($"Type {type.FullName} is not derived from {typeof(Entity).FullName}");
                else return false;
            }
            return true;
        }






        public class Poolable : IPoolable<ComponentBranch>
        {
            int IPoolable.Capacity => throw new NotImplementedException();

            ComponentBranch IPoolable<ComponentBranch>.OnCreate()
            {
                return new ComponentBranch();
            }

            void IPoolable<ComponentBranch>.OnDestroy(ComponentBranch obj)
            {

            }

            void IPoolable<ComponentBranch>.OnGet(ComponentBranch obj)
            {

            }

            void IPoolable<ComponentBranch>.OnSet(ComponentBranch obj)
            {
                foreach (var com in obj.m_Components)
                {
                    com.Value.Send<IEntityDestroy>();
                }
                obj.m_Components.Clear();
            }
        }

    }
}
