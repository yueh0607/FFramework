using FFramework.Internal;
using System;
using System.Collections.Generic;

namespace FFramework
{
    public class FEventPublisher : FUnit, IEventPublisher
    {
        internal Dictionary<Type, DynamicQueue<IEventListener>> m_EventContainer = new Dictionary<Type, DynamicQueue<IEventListener>>();

        internal void InternalAddEvent(Type type, IEventListener obj)
        {
            if (!type.IsInterface)
                throw new InvalidOperationException("Type must be an interface");

            if (!m_EventContainer.ContainsKey(type))
            {
                m_EventContainer[type] = new DynamicQueue<IEventListener>();
            }
            m_EventContainer[type].Add(obj);
        }


        public void Subscribe<T>(T obj) where T : IGenericEventBase
            => InternalAddEvent(typeof(T), obj);

        internal void InternalRemoveEvent(Type type, IEventListener obj)
        {
            if (!type.IsInterface)
                throw new InvalidOperationException("Type must be an interface");

            if (m_EventContainer.ContainsKey(type))
            {
                m_EventContainer[type].Remove(obj);
            }
        }

        public void UnSubscribe<T>(T obj) where T : IGenericEventBase
            => InternalRemoveEvent(typeof(T), obj);

        internal DynamicQueue<IEventListener> InternalGetPublishableEvents(Type type)
        {
            return m_EventContainer.ContainsKey(type) ? m_EventContainer[type] : null;
        }

        public DynamicQueue<IEventListener> GetPublishableEvents<T>(Type type) where T : IGenericEventBase
            => InternalGetPublishableEvents(type);

       
    }
}
