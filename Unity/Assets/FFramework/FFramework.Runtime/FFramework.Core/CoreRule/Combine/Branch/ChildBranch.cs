using System;
using System.Collections.Generic;

namespace FFramework
{
    public class ChildBranch : Branch
    {
        private List<Entity> m_Children;

        public int ChildCount => m_Children.Count;

        public ChildBranch()
        {
            m_Children = new List<Entity>();
        }

        public void Add<ChildType>() where ChildType : Entity
        {
            Entity child = Activator.CreateInstance<ChildType>();
            child.Branch = this;
            m_Children.Add(child);
            child.Send<IEntityAdd>();
        }

        public void Add(Type type)
        {
            CheckType(type);
            Entity child = (Entity)Activator.CreateInstance(type);
            child.Branch = this;
            m_Children.Add(child);
            child.Send<IEntityAdd>();
        }
        public void Add(Entity child)
        {
            if (!m_Children.Contains(child))
            {
                m_Children.Add(child);
                child.Send<IEntityAdd>();
            }
        }

        public bool Remove(Entity component)
        {
            component.Send<IEntityRemove>();
            return m_Children.Remove(component);
        }

        public Entity GetChild(int index)
        {
            if (index >= m_Children.Count || index < 0)
            {
                throw new IndexOutOfRangeException($"Index={index},Count={m_Children.Count}");
            }
            return m_Children[index];
        }

        public Entity GetChild(Type type)
        {
            for (int i = 0; i < m_Children.Count; ++i)
            {
                if (m_Children[i].GetType() == type)
                    return m_Children[i];
            }
            return null;
        }

        public T GetChild<T>() where T : Entity
        {
            return (T)GetChild(typeof(T));
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


        public class Poolable : IPoolable<ChildBranch>
        {
            int IPoolable.Capacity => 1000;

            ChildBranch IPoolable<ChildBranch>.OnCreate()
            {
                return new ChildBranch();
            }

            void IPoolable<ChildBranch>.OnDestroy(ChildBranch obj)
            {

            }

            void IPoolable<ChildBranch>.OnGet(ChildBranch obj)
            {

            }

            void IPoolable<ChildBranch>.OnSet(ChildBranch obj)
            {
                foreach (var com in obj.m_Children)
                {
                    com.Send<IEntityDestroy>();
                }
                obj.m_Children.Clear();
            }
        }

    }
}
