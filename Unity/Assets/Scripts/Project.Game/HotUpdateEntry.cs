using FFramework;
using FFramework.MicroAOT;
using System;
using System.Text;
using System.Threading.Tasks;
using TestAA;
using UnityEngine;


public static class HotUpdateEntry
{

    


    [EntryPriority(0)]
    public static void Main()
    {
        DynamicSequence seq = new DynamicSequence();

        MyPack pack = new MyPack()
        {
            x = 336,
            t = -480
        };

        pack.Serialize(ref seq);
        byte[] source = seq.GetMergeSequence();
        StringBuilder builder = new StringBuilder();
        foreach (var item in source)
        {
            builder.Append(item);
        }

        Debug.Log(builder.ToString());

        pack.t = 0;
        pack.x = 0;

        pack.Deserialize(ref seq);
        Debug.Log($"t={pack.t},x={pack.x}");
    }

}

