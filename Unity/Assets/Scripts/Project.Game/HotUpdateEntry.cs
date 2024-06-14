using FFramework;
using FFramework.MicroAOT;
using System.Threading.Tasks;
using UnityEngine;

public static class HotUpdateEntry
{


    [EntryPriority(0)]
    public static void Main()
    {

        Test().Forget();
    }

    static async FTask Test()
    {
        await Test2();
    }

    static async FTask Test2()
    {

        //try
        //{
            await Task.Run(() =>
            {
                throw new System.Exception("TestEx");
            }).ToFTask();

        //}
        //catch
        //{
        //    Debug.LogError("Catched");
        //}

        await FTask.DelaySeconds(3);
        Debug.Log("3秒后");
    }
}

