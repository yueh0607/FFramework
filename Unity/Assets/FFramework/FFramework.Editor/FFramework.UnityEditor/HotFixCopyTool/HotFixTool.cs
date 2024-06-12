using FFramework.Utils.Editor;
using HybridCLR.Editor.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEngine;

namespace FFramework.HotFix.Editor
{

    public class HotFixTool : EditorWindow
    {
        [MenuItem("HotFixCopyTool", menuItem = "FFramework/HotFix Copy Tool")]
        public static void OpenWindow()
        {
            var window = GetWindow<HotFixTool>();
            window.name = WINDOW_NAME;
            window.Show();
        }

        const string WINDOW_NAME = "热更新DLL拷贝工具";

        static List<string> HotUpdateAssemblyNames = new List<string>();
        static List<string> PatchMetaDataNames = new List<string>();

        Vector2 hotUpdateScrollViewPosition = Vector2.zero;
        Vector2 patchDataScrollViewPosition = Vector2.zero;

        static BuildTarget currentBuildTarget;

        const string DLL_EXTENSION = ".dll";
        const string BYTES_EXTENSION = ".bytes";


        private void OnDestroy()
        {
            HotFixToolSettings.Save();
        }


        private void OnGUI()
        {
            this.titleContent = new GUIContent($"{WINDOW_NAME} - {currentBuildTarget}");

            HotFixToolSettings.Instance.hotFixBytesPath = EditorGUILayout.TextField("热更新DLL拷贝路径", HotFixToolSettings.Instance.hotFixBytesPath);
            HotFixToolSettings.Instance.metaDataBytesPath = EditorGUILayout.TextField("补充元数据DLL拷贝路径", HotFixToolSettings.Instance.metaDataBytesPath);

            GUILayout.BeginHorizontal();
            var color = GUI.color;
            GUI.color = Color.green;
            if (GUILayout.Button("重新拷贝")) ClearAndCopyHotFixDllBytes();
            if (GUILayout.Button("重置路径")) HotFixToolSettings.Reset();
            if (GUILayout.Button("清空拷贝")) ClearHotFixDllFiles();
            GUI.color = color;
            GUILayout.EndHorizontal();

            GUILayout.Label("热更程序集列表(ReadOnly)");
            hotUpdateScrollViewPosition = GUILayout.BeginScrollView(hotUpdateScrollViewPosition);
            foreach (var item in HotUpdateAssemblyNames)
            {
                GUI.enabled = false;
                GUILayout.TextArea(item);
                GUI.enabled = true;
            }
            GUILayout.EndScrollView();


            GUILayout.Label("补充元数据列表(ReadOnly)");
            patchDataScrollViewPosition = GUILayout.BeginScrollView(patchDataScrollViewPosition);
            foreach (var item in PatchMetaDataNames)
            {
                GUI.enabled = false;
                GUILayout.TextArea(item);
                GUI.enabled = true;
            }
            GUILayout.EndScrollView();
        }

        private void Update()
        {
            LoadAllUsefulData();
        }

        static void LoadAllUsefulData()
        {
            currentBuildTarget = EditorUserBuildSettings.activeBuildTarget;

            LoadHotUpdateAssemblyNames();
            LoadMetaDataAssemblyNames();
        }

        static void LoadHotUpdateAssemblyNames()
        {
            HotUpdateAssemblyNames.Clear();
            foreach (var name in HybridCLRSettings.Instance.hotUpdateAssemblies)
            {
                HotUpdateAssemblyNames.Add(name);
            }
            foreach (var adf in HybridCLRSettings.Instance.hotUpdateAssemblyDefinitions)
            {
                HotUpdateAssemblyNames.Add(adf.name);
            }
        }
        static void LoadMetaDataAssemblyNames()
        {
            PatchMetaDataNames.Clear();
            foreach (var name in HybridCLRSettings.Instance.patchAOTAssemblies)
            {
                PatchMetaDataNames.Add(name);
            }
            foreach (var name in AOTGenericReferences.PatchedAOTAssemblyList)
            {
                PatchMetaDataNames.Add(Path.GetFileNameWithoutExtension(name));
            }
        }


        static bool HotAssemblyNameTest(string name)
        {
            return HotUpdateAssemblyNames.Contains(name);
        }
        static bool PatchMetaAssleblyNameTest(string name)
        {
            return PatchMetaDataNames.Contains(name);
        }


        public static void ClearAndCopyHotFixDllBytes()
        {

            LoadAllUsefulData();


            ClearHotFixDllFiles(false);

            string absHotUpdateDllCopyFromPath = Path.Combine(HybridCLRSettings.Instance.hotUpdateDllCompileOutputRootDir, currentBuildTarget.ToString());
            string absMetaDataDllCopyFromPath = Path.Combine(HybridCLRSettings.Instance.strippedAOTDllOutputRootDir, currentBuildTarget.ToString());


            string absHotUpdateDllCopyTargetPath = EditorPathUtils.GetAbsLocation(EPathType.AssetPath, HotFixToolSettings.Instance.hotFixBytesPath);
            string absPatchMetaDataDllCopyTargetPath = EditorPathUtils.GetAbsLocation(EPathType.AssetPath, HotFixToolSettings.Instance.metaDataBytesPath);


            int i = FolderBytesCopyer.CopyBytes(absHotUpdateDllCopyFromPath,
                DLL_EXTENSION, absHotUpdateDllCopyTargetPath, BYTES_EXTENSION, HotAssemblyNameTest);
            Debug.Log($"Copy HotUpdateDll Completed ,Count: {i}");

            int j = FolderBytesCopyer.CopyBytes(absMetaDataDllCopyFromPath,
                DLL_EXTENSION, absHotUpdateDllCopyTargetPath, BYTES_EXTENSION, PatchMetaAssleblyNameTest);
            Debug.Log($"Copy MetaDataDll Completed: Count {j}");

            AssetDatabase.Refresh();
            
           
        }

        public static void ClearHotFixDllFiles(bool needRefresh = true)
        {
            string absHotUpdateDllCopyTargetPath = EditorPathUtils.GetAbsLocation(EPathType.AssetPath, HotFixToolSettings.Instance.hotFixBytesPath);
            string absPatchMetaDataDllCopyTargetPath = EditorPathUtils.GetAbsLocation(EPathType.AssetPath, HotFixToolSettings.Instance.metaDataBytesPath);
            FolderBytesCopyer.ClearFiles(absHotUpdateDllCopyTargetPath);
            Debug.Log($"Clear HotUpdateDll Completed ");
            FolderBytesCopyer.ClearFiles(absPatchMetaDataDllCopyTargetPath);
            Debug.Log($"Clear MetaDataDll Completed");
            if (needRefresh) AssetDatabase.Refresh();

        }


    }
}