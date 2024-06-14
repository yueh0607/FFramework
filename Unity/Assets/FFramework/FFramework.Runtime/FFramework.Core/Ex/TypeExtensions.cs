using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace FFramework
{
    public static partial class TypeExtensions
    {

        /// <summary>
        /// 抽象则抛异常
        /// </summary>
        /// <param name="type"></param>
        /// <exception cref="ArgumentException"></exception>
        //[DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IfAbstractThrowException(this Type type)
        {
            if (type.IsAbstract) throw new ArgumentException("Type parameters are not allowed to be abstract");
        }

        /// <summary>
        /// (High Loss) 获取显示名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetDisplayName(this Type type)
        {
            if (!type.IsGenericType)
                return type.Name;

            StringBuilder sb = new StringBuilder();

            // 获取类型名称
            string typeName = type.Name.Substring(0, type.Name.IndexOf('`'));

            sb.Append(typeName);
            sb.Append("<");

            Type[] genericArguments = type.GetGenericArguments();
            for (int i = 0; i < genericArguments.Length; i++)
            {
                if (i > 0)
                    sb.Append(", ");
                sb.Append(genericArguments[i].Name);
            }
            sb.Append(">");

            return sb.ToString();
        }

        /// <summary>
        /// 获取此类的全部子类(High Loss)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetSubclassesOf(this Type type, Predicate<Type> predicate = null)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p != type)
                .Where(x => predicate == null || predicate(x));
        }
        /// <summary>
        /// 获取此类的全部子类(High Loss)，包含此类
        /// </summary>
        /// <param name="type"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetSubclassesOfIncludeThis(this Type type, Predicate<Type> predicate = null)
        {
            List<Type> types = new List<Type>
            {
                type
            };
            types.AddRange(GetSubclassesOf(type,predicate));
            return types;
        }

        /// <summary>
        /// 判断一个类型 是否是   另一个“泛型”类   的派生类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="genericType"></param>
        /// <returns></returns>
        public static bool IsSubclassOfRawGeneric(this Type type, Type genericType)
        {
            while (type != null && type != typeof(object))
            {
                var currentType = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
                if (genericType == currentType)
                    return true;
                type = type.BaseType;
            }
            return false;
        }

        public static bool HasInterface(this Type type, Type _interface)
        {
            return _interface.IsAssignableFrom(type);
        }

        /// <summary>
        /// 获取泛型类的名字，不含参数
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetGenericTypeName(this Type type)
        {
            return type.Name;
        }
    }


}

