using System;
using System.Collections.Generic;

namespace FFramework
{
    [ModuleStatic("FModel")]
    [ModuleVague]
    public class ModelModule : IModule
    {
        private Dictionary<KeyValuePair<Type,long>, IModel> m_GlobalModel = new Dictionary<KeyValuePair<Type, long>, IModel>();

        internal IModel InternalGetModel(Type type,long id = 0)
        {
            KeyValuePair<Type,long> key = new KeyValuePair<Type,long>(type,id);
            if (m_GlobalModel.TryGetValue(key, out IModel existedModel))
            {
                return existedModel;
            }
            var model = (IModel)Activator.CreateInstance(type);
            m_GlobalModel.Add(key, model);
            return model;
        }

        public T GetModel<T>(long id = 0) where T : IModel
        {
            return (T)InternalGetModel(typeof(T),id);
        }


        void IModule.OnCreate(object moduleParameter)
        {

        }

        void IModule.OnDestroy()
        {

        }
    }
}
