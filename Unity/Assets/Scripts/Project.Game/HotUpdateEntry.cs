using FFramework;
using FFramework.MicroAOT;
using System;
using System.Threading.Tasks;
using UnityEngine;

public static class HotUpdateEntry
{

    static async FTask Test()
    {


        await FTask.DelaySeconds(3);
        Debug.Log("1");


        await Task.Run(async () =>
        {
            await Task.Delay(3000);
            //throw new System.Exception("主动抛出的异常");
        }).ToFTask();


        await FTask.DelaySeconds(3);
        Debug.Log("3");
    }

    static FCancellationToken token = new FCancellationToken();

    [EntryPriority(0)]
    public static void Main()
    {
        Test().Forget(token);

        token.CancelAfterSeconds(1);

    }

}

