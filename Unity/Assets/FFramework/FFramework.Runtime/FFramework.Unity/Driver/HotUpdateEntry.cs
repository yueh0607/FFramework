using FFramework;
using UnityEngine;

public static class HotUpdateEntry
{
    private static GameObject m_FrameworkRoot;
    public static GameObject Root => m_FrameworkRoot;

    public static void Main()
    {
        m_FrameworkRoot = new GameObject("[FFramework]");
        GameObject.DontDestroyOnLoad(m_FrameworkRoot);

        m_FrameworkRoot.AddComponent<UnityDriver>();
    }
}

