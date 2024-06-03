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

        await FTask.DelaySeconds(3);

        Debug.Log($"A捕获到的令牌ID：{token.ID}");

  
    }

    FCancellationTokenHolder token1 = new FCancellationTokenHolder();
 

    
    public void Start()
    {

        Debug.Log($"传入A的令牌ID：{token1.ID}");
        A().Forget(token1);
        token1.CancelAfterSeconds(1.5f);
    }
}
