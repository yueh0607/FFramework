using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFramework
{
    public class AOTSingleton<T> where T : AOTSingleton<T>
    {
        private static T m_Instance;

        protected AOTSingleton(){}

        public static T Instance
        {
            get
            {
                m_Instance ??= Activator.CreateInstance<T>();
                return m_Instance;
            }
        }

        public static void Destory()
        {
            m_Instance = null;
        }
    }
}
