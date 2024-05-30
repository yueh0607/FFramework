using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFramework
{
    [AttributeUsage(AttributeTargets.Method)]
    public class BindAttribute : System.Attribute
    {
        private string m_BindName;
        private string m_EventName;
        public string BindName => m_BindName;
        public string EventName => m_EventName;
        public BindAttribute(string bindName,string eventName)
        {
            m_BindName = bindName;
            m_EventName = eventName;
        }
    }
}
