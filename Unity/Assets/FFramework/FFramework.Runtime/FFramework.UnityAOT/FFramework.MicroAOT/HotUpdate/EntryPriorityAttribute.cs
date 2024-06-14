using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFramework.MicroAOT
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class EntryPriorityAttribute : Attribute
    {
        public int Priority { get; private set; }
        /// <summary>
        /// 优先级越大，越先执行
        /// </summary>
        /// <param name="priotity"></param>
        public EntryPriorityAttribute(int priotity)
        {
            Priority = priotity;
        }
    }
}
