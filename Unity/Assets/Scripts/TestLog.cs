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
        //token.Cancel();
        //if (token.IsCancellationRequested) return;
        Debug.Log($"捕获的令牌ID：{token.ID}");

        await FTask.DelaySeconds(3);
        Debug.Log($"3s");
    }

    FCancellationToken token1 = new FCancellationToken();
 
    public void Start()
    {
        FTask.Tick(TimeSpan.FromSeconds(1), (time) => Debug.Log($"现在是第{time.TotalSeconds}秒")).CancelAfterSeconds(10);

        Debug.Log($"传入A的令牌ID：{token1.ID}");
        A().Forget(token1);
        token1.SuspendAfterSeconds(1f);
        token1.RestoreAfterSeconds(5);
        
    }
}
