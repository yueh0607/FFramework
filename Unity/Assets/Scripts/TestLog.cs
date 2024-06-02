using FFramework;
using System;
using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;


public class TestLog : MonoBehaviour
{
 
    async FTask A()
    {
        var token = await FTask.CatchToken();
        Debug.Log($"A捕获到的令牌ID：{token.ID}");
        var task = B();
        Debug.Log($"传入B的令牌ID：{token2.ID}");
        ((IFTaskAwaiter)task.GetAwaiter()).SetToken(token2);
        await task;
        
    }
    async FTask B()
    {
        var token = await FTask.CatchToken();
        Debug.Log($"B捕获到的令牌ID：{token.ID}");
        await FTask.Delay(System.TimeSpan.FromSeconds(5));
        Debug.Log("!!");
    }

    FCancellationTokenHolder token1 = new FCancellationTokenHolder();
    FCancellationTokenHolder token2 = new FCancellationTokenHolder();

    public void Start()
    {

        Debug.Log($"传入A的令牌ID：{token1.ID}");
        A().Forget(token1);
    }
}
