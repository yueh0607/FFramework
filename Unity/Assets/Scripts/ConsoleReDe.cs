using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class ConsoleToUnityLogger
{
    private class UnityTextWriter : TextWriter
    {
        public override Encoding Encoding => Encoding.UTF8;

        public override void Write(string value)
        {
            Debug.Log(value);
        }

        public override void WriteLine(string value)
        {
            Debug.Log(value);
        }
    }

    static ConsoleToUnityLogger()
    {
        // Redirect Console output to Unity's Debug.Log
        Console.SetOut(new UnityTextWriter());
        Console.SetError(new UnityTextWriter());
    }

    public static void Initialize()
    {
        // This method ensures the static constructor is called
    }
}
