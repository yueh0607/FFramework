using FFramework;
using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;


public class TestLog : MonoBehaviour

{



  

    public void Start()
    {
        Debug.Log($"主线程ID: {Thread.CurrentThread.ManagedThreadId}");

        SynchronizationContext.Current.Post((x) =>
        {
            Debug.Log($"从主线程Post到的线程ID: {Thread.CurrentThread.ManagedThreadId}");
        }, null);

 
     


    }


}
