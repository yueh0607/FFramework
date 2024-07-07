using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace FFramework
{
    //环境：每个Envirment必须独占一个线程、独占一个框架核心，以保证无锁并发
    public abstract partial class Envirment : FDispoableUnit
    {
        public abstract FSynchronizationContext MailBox { get; protected set; }

        private EventModule m_EventModule;

        private Thread m_Thread;

        public Thread BindThread => m_Thread;

        protected void RegisterEnvirment(Thread thread)
        {
            if (!m_Env.TryAdd(thread.ManagedThreadId, this))
                throw new System.Exception("Envirment must be unique in a thread");
            m_Thread = thread;

            if (MailBox == null)
                throw new System.NullReferenceException("MailBox is null");

            MailBox.BelongEnvirment = this;
            m_EventModule.Publisher.Subscribe<IUpdate>(MailBox);
        }

        protected void RegisterEnvirment()
        {
            RegisterEnvirment(Thread.CurrentThread);
        }

        public Envirment()
        {
            CreateModule<EventModule>(null);
            CreateModule<PoolModule>(null);

            m_EventModule = GetModule<EventModule>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Post(IMessagePack message)
        {
            MailBox.Post(message);
        }

        protected override void OnReleaseManagedResource()
        {
            //注销邮箱
            m_EventModule.Publisher.UnSubscribe<IUpdate>(MailBox);
            MailBox = null;
            //销毁模块
            foreach (var module in m_ContextModules)
            {
                module.Value.OnDestroy();
            }
            m_ContextModules.Clear();
            m_ContextModules = null;
        }

        protected override void OnReleaseUnmanagedResource()
        {

        }
    }

    public abstract partial class Envirment
    {
        private Dictionary<Type, IModule> m_ContextModules = new Dictionary<Type, IModule>();

        private List<Type> m_ModuleTypeCache = new List<Type>();
        public void CreateModule<T>(object moduleParameter = null) where T : IModule, new()
        {
            m_ModuleTypeCache.Clear();
            Type moduleType = typeof(T);
            m_ModuleTypeCache.Add(moduleType);

            if (m_ContextModules.ContainsKey(typeof(T)))
                throw new ArgumentException("Module with the same ID already exists.", moduleType.BaseType.FullName);
            IModule module = new T();
           
            m_ContextModules.Add(typeof(T), module);


            while (moduleType.BaseType != null)
            {

                if (!typeof(IModule).IsAssignableFrom(moduleType.BaseType) || moduleType.BaseType.GetCustomAttribute<ModuleVagueAttribute>() == null)
                {
                    moduleType = moduleType.BaseType;
                    continue;
                }

                if (m_ContextModules.ContainsKey(typeof(T)))
                    throw new ArgumentException("Module with the same ID already exists.", moduleType.BaseType.FullName);
                
                m_ContextModules.Add(moduleType.BaseType, module);
                moduleType = moduleType.BaseType;

            }
            module.OnCreate(moduleParameter);
        }

        public void DestroyModule<T>()
        {
            if (m_ContextModules.TryGetValue(typeof(T), out IModule module))
            {
                module.OnDestroy();
                m_ContextModules.Remove(typeof(T));
            }
            else
            {
                // 如果未找到具有给定模块ID的模块，则抛出异常或者采取其他适当的处理方式
                throw new KeyNotFoundException($"Module with ID '{typeof(T).FullName}' not found.");
            }
        }

        public void CheckDependence<T>() where T : IModule
        {
            if (!m_ContextModules.ContainsKey(typeof(T)))
            {
                throw new KeyNotFoundException($"Module with ID '{typeof(T).FullName}' not found.");
            }
        }

        public T GetModule<T>() where T : IModule
        {
            if (m_ContextModules.TryGetValue(typeof(T), out IModule module))
            {
                return (T)module;
            }
            else
            {
                throw new KeyNotFoundException($"Module with ID '{typeof(T).FullName}' not found.");
            }
        }

    }



    public abstract partial class Envirment
    {
        protected static ConcurrentDictionary<int, Envirment> m_Env = new ConcurrentDictionary<int, Envirment>();
        public static Envirment Current => GetEnvirment(Thread.CurrentThread.ManagedThreadId);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Envirment GetEnvirment(Thread thread)
        {
            return GetEnvirment(thread.ManagedThreadId);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Envirment GetEnvirment(int threadId)
        {
            if (!m_Env.ContainsKey(threadId))
                throw new System.NullReferenceException("Envirment not found in thread");

            return m_Env[threadId];
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DestroyEnvirment(Thread thread)
        {
            DestroyEnvirment(thread.ManagedThreadId);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DestroyEnvirment(int threadId)
        {
            if (m_Env.TryRemove(Thread.CurrentThread.ManagedThreadId, out Envirment env))
            {
                env.Dispose();
            }
        }

    }


    public static class ThreadExtension
    {
        public static Envirment GetFFrameworkEnvirment(this Thread thread)
        {
            return Envirment.GetEnvirment(thread.ManagedThreadId);
        }
    }
}
