using FFramework;
using System;
using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;


public class TestLog : MonoBehaviour
{
    float time = 0;
    Stopwatch stopwatch;
    async FTask TimeStep()
    {
        stopwatch = Stopwatch.StartNew();
        Debug.Log($"当前时刻: {0}s末  真正时刻:{stopwatch.Elapsed.TotalSeconds} Update时刻:{time}");
        for (int i = 1; i < 100; i++)
        {
            await FTask.Delay(System.TimeSpan.FromSeconds(1f), ETimerLoop.Update);
            Debug.Log($"当前时刻: {i}s末  真正时刻:{stopwatch.Elapsed.TotalSeconds} Update时刻:{time}");
        }
    }

    private void Update()
    {
        time += Time.deltaTime;
    }

    ThreadEnvirment env = new ThreadEnvirment(30);

    object lockedAsset = new object();
    async FTask DO()
    {
        await FTask.Delay(System.TimeSpan.FromSeconds(5));
        Debug.Log("这是应该在第5s初完成的任务");
        var cur = Envirment.Current;
        int a = 0;
        await FTask.SwitchThread(env.MailBox);
        a = 33;
        await FTask.SwitchThread(cur.MailBox);
        Debug.LogError(a);
        await FTask.Delay(System.TimeSpan.FromSeconds(3));
        Debug.Log("这是应该在第8s初完成的任务");
    }
    async FTask DO2()
    {
        await FTask.LockAsset(lockedAsset);
        await FTask.Delay(System.TimeSpan.FromSeconds(5));
        Debug.Log("这是应该在第5s初完成的任务");

        await FTask.Delay(System.TimeSpan.FromSeconds(3));
        Debug.Log("这是应该在第8s初完成的任务");
        await FTask.UnlockAsset(lockedAsset);
    }


    async Task DO3()
    {
        await Task.Delay(5000);
    }

    public void Start()
    {

        TimeStep().Forget();
        DO().Forget();
        //DO2().Forget();
    }
}
