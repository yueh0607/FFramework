using FFramework;
using FFramework.MicroAOT;
using System.Threading.Tasks;
using UnityEngine;

public static class HotUpdateEntry
{

    static async FTask Test()
    {
        
        await FTask.DelaySeconds(3);
        Debug.Log("1");
        await FTask.DelaySeconds(3);
        Debug.Log("2");
    }

    static FCancellationToken token = new FCancellationToken();

    [EntryPriority(0)]
    public static void Main()
    {
        Test().Forget(token);

        token.CancelAfterSeconds(4);
        
    }

}

