using FFramework;
using FFramework.MicroAOT;
using UnityEngine;

public static class HotUpdateEntry
{
    private static GameObject m_FrameworkRoot;
    public static GameObject Root => m_FrameworkRoot;

    public static UnityEnvirment Env { get; private set; }

    [EntryPriority(int.MaxValue)]
    public static void Main()
    {
        Env = new UnityEnvirment();
        FLoger.SetLogger(new FUnityLogger());

        m_FrameworkRoot = new GameObject("[FFramework]");
        GameObject.DontDestroyOnLoad(m_FrameworkRoot);
       
        m_FrameworkRoot.AddComponent<UnityDriver>();

        FLoger.LogError("测试");
    }

}

