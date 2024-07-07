using System;

namespace FFramework
{
    public interface IEntity : IUnique
    {
        public IModel GetModel(Type type);
        public T GetModel<T>() where T : IModel;
    }

}
