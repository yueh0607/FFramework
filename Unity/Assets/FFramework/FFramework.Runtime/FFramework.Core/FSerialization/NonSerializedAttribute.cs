using System;

namespace FFramework.Serialization
{

    /// <summary>
    /// 指示字段、属性在序列化时忽略
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class NonSerialized : Attribute
    {
    }
}
