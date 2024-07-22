using FFramework;
using FFramework.MicroAOT;
using System;
using System.Threading.Tasks;
using UnityEngine;

public partial class TestInject
{
    [Inject]
    public int a = 0;
}

public static class HotUpdateEntry
{

    

    static async FTask Test()
    {
        Scope scope = new Scope();
        scope.Register<int>(10);

        TestInject a = new TestInject();
        a.Inject();

        await FTask.DelaySeconds(1);

        Debug.Log(a.a);
    }

    static FCancellationToken token = new FCancellationToken();

    [EntryPriority(0)]
    public static void Main()
    {
        Test().Forget(token);

        token.CancelAfterSeconds(1);

    }

}

