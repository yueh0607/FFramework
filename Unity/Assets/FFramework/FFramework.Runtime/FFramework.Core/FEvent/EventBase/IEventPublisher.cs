using FFramework.Internal;
using System;
using System.Collections.Generic;

namespace FFramework
{
    public interface IEventPublisher
    {
     
        void Subscribe<T>(T obj) where T : IGenericEventBase;
        

        void UnSubscribe<T>(T obj) where T : IGenericEventBase;

        DynamicQueue<IEventListener> GetPublishableEvents<T>(Type type) where T : IGenericEventBase;
    }
}
