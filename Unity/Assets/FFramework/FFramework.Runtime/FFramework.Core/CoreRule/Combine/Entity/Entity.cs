using System;
using System.Collections.Generic;

namespace FFramework
{
    public abstract class Entity : FUnit, IEntity
    {
        public Branch Branch { get; internal set; } = null;

        public Entity()
        {

        }

        ~Entity()
        {

        }

        public IModel GetModel(Type type)
        {
            return Envirment.Current.GetModule<ModelModule>().InternalGetModel(type, this.ID);
        }

        public T GetModel<T>() where T : IModel
        {
            return (T)GetModel(typeof(T));
        }

    }



}
