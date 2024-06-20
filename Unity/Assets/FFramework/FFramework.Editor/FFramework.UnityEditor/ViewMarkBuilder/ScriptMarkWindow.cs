using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;

namespace FFramework.ViewMark.Editor
{
    public class ScriptMarkWindow : EditorWindow
    {
        [MenuItem("FFramework/ScriptMarkTool")]
        public static void ShowWindow()
        {
            GetWindow<ScriptMarkWindow>("Script Mark Window");
        }

        private void OnGUI()
        {
            DrawSceneRootObjects();

            DrawPrefabRootObjects();

        }

        Vector2 sceneObjectPostion = Vector2.zero;
        bool sceneObjectFoldout = true;
        Vector2 prefabObjectPostion = Vector2.zero;
        bool prefabObjectFoldout = true;
        private void DrawSceneRootObjects()
        {
            GameObject[] rootObjects = GetSceneRootObjects();
            if (sceneObjectFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(sceneObjectFoldout, $"SceneObjects({rootObjects.Length})"))
            {
                EditorGUILayout.BeginScrollView(sceneObjectPostion);
                foreach (GameObject root in rootObjects)
                {
                    DrawObjectField(root);
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private GameObject[] GetSceneRootObjects()
        {
            List<GameObject> rootObjects = new List<GameObject>();
            Scene scene = SceneManager.GetActiveScene();

            foreach (GameObject go in scene.GetRootGameObjects())
            {
                if (go.GetComponent<ScriptMark>() != null)
                {
                    rootObjects.Add(go);
                }
            }

            return rootObjects.ToArray();
        }

        private List<GameObject> prefabObjectsWithScriptMark = new List<GameObject>();

        private void DrawPrefabRootObjects()
        {
            // 只有在没有缓存的情况下或者刷新时重新计算
            if (prefabObjectsWithScriptMark.Count == 0)
            {
                CachePrefabsWithScriptMark();
            }

            // 显示折叠组并统计数量
            if (prefabObjectFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(prefabObjectFoldout, $"Prefabs ({prefabObjectsWithScriptMark.Count})"))
            {
                prefabObjectPostion = EditorGUILayout.BeginScrollView(prefabObjectPostion, GUILayout.Height(200));

                foreach (GameObject prefab in prefabObjectsWithScriptMark)
                {
                    DrawObjectField(prefab);
                }

                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private void CachePrefabsWithScriptMark()
        {
            prefabObjectsWithScriptMark.Clear(); // 清空缓存

            string[] prefabPaths = AssetDatabase.FindAssets("t:Prefab");

            foreach (string prefabPath in prefabPaths)
            {
                string path = AssetDatabase.GUIDToAssetPath(prefabPath);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                if (prefab != null && prefab.GetComponent<ScriptMark>() != null)
                {
                    prefabObjectsWithScriptMark.Add(prefab);
                }
            }
        }

     

        void DrawObjectField(GameObject obj)
        {
            EditorGUILayout.ObjectField(obj, typeof(GameObject), true);
        }
    }
}
