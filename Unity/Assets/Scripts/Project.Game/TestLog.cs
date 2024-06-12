using FFramework;
using System;
using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;


public class TestLog : MonoBehaviour, IProgress<float>
{

    async FTask A()
    {
        ResourceInitParameters param = new ResourceInitParameters.Simulate();

        await FResource.Initialize(param);
        var loadhandle = FResource.LoadAssetAsync<GameObject>("UIRoot");
        await loadhandle.EnsureDone(this);
    }

    async FTask C()
    {
        var token = await FTask.CatchToken();
        Debug.Log($"C捕获的ID：{token.ID}");
    }

    async FTask B()
    {
        var token = await FTask.CatchToken();
        Debug.Log($"B捕获的ID：{token.ID}");
        await C();
        await FTask.DelaySeconds(1);
    }


    FCancellationToken token1 = new FCancellationToken();

    public void Start()
    {
        FTask.Tick(TimeSpan.FromSeconds(1), (time) => Debug.Log($"现在是第{time.TotalSeconds}秒")).CancelAfterSeconds(100);
        Debug.Log($"传入A的令牌ID：{token1.ID}");

        Envirment.Current.CreateModule<UnityResourceModule>();
        B().Forget(token1);
        A().Forget(token1);


    }

    public void Report(float value)
    {
        Debug.Log("加载进度：" + value);
    }
}
