using FFramework;
using System;
using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;


public class TestLog : MonoBehaviour
{
 
    async FTask DO()
    {
        var token = await FTask.CatchToken();
        Debug.Log($"DO捕获到的令牌ID：{token.ID}");
        if (false)
        {
            await FTask.Delay(TimeSpan.FromSeconds(3));
        }
        await DO2();
        
    }
    async FTask DO2()
    {
        var token = await FTask.CatchToken();
        Debug.Log($"DO2捕获到的令牌ID：{token.ID}");
        await FTask.Delay(System.TimeSpan.FromSeconds(5));
        Debug.Log("!!");
    }

    FCancellationTokenHolder tokenHolder = new FCancellationTokenHolder();

    public void Start()
    {
        Debug.Log($"传入的令牌ID：{tokenHolder.ID}");
        DO().Forget(tokenHolder);
    }
}
