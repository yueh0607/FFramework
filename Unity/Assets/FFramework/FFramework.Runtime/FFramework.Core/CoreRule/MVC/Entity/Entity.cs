using System;
using System.Collections.Generic;

namespace FFramework
{
    public abstract class Entity : FUnit, IEntity
    {

        public IBranch Branch { get; internal set; }

        private List<Component> m_Components;
        
        public Entity()
        {
            Branch = null;
            m_Components = Envirment.Current.GetModule<PoolModule>().Get<List<Component>, ListPoolable<List<Component>>>();
        }

        ~Entity()
        {
            Envirment.Current.GetModule<PoolModule>().Set<List<Component>, ListPoolable<List<Component>>>(m_Components);
        }

        internal IModel GetModel(Type type)
        {
            return Envirment.Current.GetModule<ModelModule>().InternalGetModel(type, this.ID);
        }

        public T GetModel<T>() where T : IModel
        {
            return (T)GetModel(typeof(T));
        }

    }


    
}
