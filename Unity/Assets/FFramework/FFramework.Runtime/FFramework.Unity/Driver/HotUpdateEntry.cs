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
        m_FrameworkRoot = new GameObject("[FFramework]");
        GameObject.DontDestroyOnLoad(m_FrameworkRoot);
        Env = new UnityEnvirment();
        m_FrameworkRoot.AddComponent<UnityDriver>();
    }

}

