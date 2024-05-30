using System;

namespace FFramework
{
    /// <summary>
    ///
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class SerializeProviderAttribute : System.Attribute
    {
        private Type m_ForType;

        public Type ForType => m_ForType;
        public SerializeProviderAttribute(Type type)
        {
            this.m_ForType = type;
        }
    }
}
