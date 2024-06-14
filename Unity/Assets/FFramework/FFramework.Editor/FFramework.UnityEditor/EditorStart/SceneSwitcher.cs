using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace UnityToolbarExtender.Examples
{

    [InitializeOnLoad]
    public sealed class SceneSwitchLeftButton
    {
        private static List<(string sceneName, string scenePath)> m_Scenes;
        private static string[] m_SceneName;
        private static List<string> m_SceneNameIndex;
        private static string[] m_ScenePath;
        private static int sceneSelected = 0;


        static SceneSwitchLeftButton()
        {


            void UpdateCurrent()
            {
                m_Scenes = SceneHelper.GetAllScenesInProject();
                m_SceneName = m_Scenes.Select((x) => x.sceneName).Append(string.Empty).ToArray();
                m_SceneNameIndex = m_Scenes.Select((x) => x.sceneName).Append(string.Empty).ToList();
                m_ScenePath = m_Scenes.Select((x) => x.scenePath).Append(string.Empty).ToArray();
            }
            EditorApplication.projectChanged += UpdateCurrent;

            UpdateCurrent();

            ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
        }

        static void OnToolbarGUI()
        {
           
            sceneSelected = m_SceneNameIndex.IndexOf(EditorSceneManager.GetActiveScene().name);
            if (sceneSelected < 0)
            {
                sceneSelected = m_SceneName.Length-1;
            }

            int sceneSelectedNew = EditorGUILayout.Popup(sceneSelected, m_SceneName);

            if (sceneSelectedNew != sceneSelected && sceneSelectedNew != m_SceneName.Length-1)
            {
                //Debug.Log("场景变更");
                SceneHelper.PromptSaveCurrentScene();
                EditorSceneManager.OpenScene(m_ScenePath[sceneSelectedNew]);
            }

        }
    }

    static class SceneHelper
    {

        public static bool PromptSaveCurrentScene()
        {
            // 检查当前场景是否已保存
            if (EditorSceneManager.GetActiveScene().isDirty)
            {
                // 提示用户是否要保存当前场景
                bool saveScene = EditorUtility.DisplayDialog(
                    "Save Current Scene",
                    "The current scene has unsaved changes. Do you want to save it?",
                    "Save",
                    "Cancel"
                );

                // 如果用户选择“保存”，则保存当前场景
                if (saveScene)
                {
                    EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                }

                return saveScene;
            }

            // 如果场景已保存或者用户选择了“取消”，则返回 true，表示继续执行后续操作
            return true;
        }

        /// <summary>
        /// 获取项目中所有的场景文件，并以 (场景名, 场景路径) 的形式返回。
        /// </summary>
        public static List<(string sceneName, string scenePath)> GetAllScenesInProject()
        {
            List<(string sceneName, string scenePath)> scenes = new List<(string sceneName, string scenePath)>();

            // 查找所有场景文件
            string[] guids = AssetDatabase.FindAssets("t:Scene");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(path);
                scenes.Add((sceneName, path));
            }

            return scenes;
        }
    }
}