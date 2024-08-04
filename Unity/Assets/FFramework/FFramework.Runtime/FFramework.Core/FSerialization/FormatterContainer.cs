using System;
using System.Collections.Generic;

namespace FFramework.Serialization.Binary
{

    public class FormatterContainter : IModule
    {
  
        private Dictionary<Type, IFSerializationFormatter> m_Container = new Dictionary<Type, IFSerializationFormatter>();

        /// <summary>
        /// 注册或者替换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="formatter"></param>
        /// <exception cref="NullReferenceException"></exception>
        public void RegisterOrReplace<T>(IFSerializationFormatter formatter)
        {
            if(formatter==null) throw new NullReferenceException("formatter is nulls");
            if (m_Container.ContainsKey(typeof(T)))
            {
                m_Container[typeof(T)] = formatter;
            }
            else
            {
                m_Container.Add(typeof(T), formatter);
            }
        }
        /// <summary>
        /// 注册或者忽略
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="formatter"></param>
        public void RegisterOrIgnore<T>(IFSerializationFormatter formatter)
        {
            if (!m_Container.ContainsKey(typeof(T)))
            {
                m_Container.Add(typeof(T), formatter);
            }
        }

        public IFSerializationFormatter GetFormatter(Type type)
        {
            if (m_Container.ContainsKey(type))
            {
                return m_Container[type];
            }
            return null;
        }
        public FSerializationFormatter<T> GetFormatter<T>()
        {
            if (m_Container.ContainsKey(typeof(T)))
            {
                return (FSerializationFormatter<T>)m_Container[typeof(T)];
            }
            else throw new InvalidOperationException("Formatter not found");
        }

        void IModule.OnCreate(object moduleParameter)
        {
            //可变长编码整数
            RegisterOrIgnore<byte>(new ByteFormatter());
            RegisterOrIgnore<sbyte>(new SByteFormatter());
            RegisterOrIgnore<short>(new ShortFormatter());
            RegisterOrIgnore<ushort>(new UShortFormatter());
            RegisterOrIgnore<int>(new IntFormatter());
            RegisterOrIgnore<uint>(new UIntFormatter());
            RegisterOrIgnore<long>(new LongFormatter());
            RegisterOrIgnore<ulong>(new ULongFormatter());

            //浮点数
            RegisterOrIgnore<float>(new UnmanagedFormatter<float>());
            RegisterOrIgnore<double>(new UnmanagedFormatter<double>());

            //基础类型补充
            RegisterOrIgnore<bool>(new UnmanagedFormatter<bool>());
            RegisterOrIgnore<char>(new UnmanagedFormatter<char>());

            //时间
            RegisterOrIgnore<DateTime>(new UnmanagedFormatter<DateTime>());
            RegisterOrIgnore<TimeSpan>(new UnmanagedFormatter<TimeSpan>());
        }

        void IModule.OnDestroy()
        {
            m_Container.Clear();
        }
    }
}
