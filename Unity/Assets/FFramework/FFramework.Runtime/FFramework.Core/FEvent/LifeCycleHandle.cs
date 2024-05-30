//using FFramework.Internal;

//namespace FFramework
//{

//    public interface ILifeCycleHandle
//    {
//        void Subscribe(object obj);
//        void Unsubscribe(object obj);
//    }

//    public sealed class LifeCycleHandle<T> : 
//        Singletion<LifeCycleHandle<T>> ,ILifeCycleHandle
//        where T : IGenericEventBase
//    {
//        public void Subscribe(object obj)
//            => FEvent.Publisher.Subscribe<T>((T)obj);

//        public void Unsubscribe(object obj)
//            => FEvent.Publisher.UnSubscribe<T>((T)obj);
//    }
//}
