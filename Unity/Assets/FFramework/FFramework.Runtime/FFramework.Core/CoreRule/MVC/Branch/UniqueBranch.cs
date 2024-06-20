using System.Collections.Generic;

namespace FFramework
{
    public class UniqueBranch<Key> : Branch, IUniqueBranch<Key>, IEnumerable<KeyValuePair<Key, Entity>>
    {

        Dictionary<Key, Entity> m_Entities;


        public UniqueBranch()
        {
            m_Entities = Envirment.Current.GetModule<PoolModule>().Get<Dictionary<Key, Entity>, DictionaryPoolable<Dictionary<Key, Entity>>>();
        }

        ~UniqueBranch()
        {
            Envirment.Current.GetModule<PoolModule>().Set<Dictionary<Key, Entity>, DictionaryPoolable<Dictionary<Key, Entity>>>(m_Entities);
        }

        public override IEnumerator<Entity> GetEnumerator()
        {
            return m_Entities.Values.GetEnumerator();
        }

        IEnumerator<KeyValuePair<Key, Entity>> IEnumerable<KeyValuePair<Key, Entity>>.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<Key, Entity>>)m_Entities).GetEnumerator();
        }


        internal void InternalAddEntity()
        {

        }

    }


    public static class UniqueBranchExtension
    {
        //public static void AddComponent<T>(this T branch, Component<T> component) where T : UniqueBranch
        //{
        //    branch.InternalAddComponent(component);
        //}

        //public static void UncheckAddComponent(this Entity entity, Component component)
        //{
        //    entity.InternalAddComponent(component);
        //}

        //public static void RemoveComponent<T>(this T entity, Component<T> component) where T : Entity
        //{
        //    entity.InternalRemoveComponent(component);
        //}

        //public static void UncheckRemoveComponent(this Entity entity, Component component)
        //{
        //    entity.InternalRemoveComponent(component);
        //}

        //public static bool TryGetComponent<T, K>(this K entity, out T component) where T : Component<K> where K : Entity
        //{
        //    T gotComponent = entity.InternalGetComponent<T>();
        //    if (gotComponent != null)
        //    {
        //        component = gotComponent;
        //        return true;
        //    }
        //    component = null;
        //    return false;
        //}

        //public static T GetComponent<T>(this Entity entity) where T : Component
        //{
        //    return entity.InternalGetComponent<T>();
        //}

        //public static Component GetComponent(this Entity entity, Type type)
        //{
        //    return entity.InternalGetComponent(type);
        //}
    }

}
