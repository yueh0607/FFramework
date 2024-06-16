using System.IO;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using FFramework.Utils.Editor;

namespace FFramework.FUnityEditor
{
    public class MainFrameworkWindow : EditorWindow
    {

        [MenuItem("MainFrameworkWindow", menuItem = "FFramework/About", priority = 0)]
        static void OpenWindow()
        {
            var window = GetWindow<MainFrameworkWindow>();
            window.titleContent = new GUIContent("FFramework");
            window.minSize = new Vector2(600, 400);
            window.Show();
        }


        Texture logo;
        private void OnEnable()
        {
            logo = LoadTextureByIO(EditorPathUtils.GetAbsLocation(EPathType.AssetPath, "FFramework/FFramework.Editor/FFramework.EditorResource/Logo.png"));
            if (logo == null)
                throw new InvalidDataException("读取不到框架logo，打开失败");
        }


        private void OnGUI()
        {
        
            // 获取顶端区域的矩形范围
            Rect rect = GUILayoutUtility.GetRect(0, position.width, 0, logo.height);

            // 绘制图片
            GUI.DrawTexture(rect, logo, ScaleMode.ScaleAndCrop, true);
            string introducation = "FFramework是一款基于C#编程语言、兼容Unity3D开发环境的模块化游戏开发框架";
            GUILayout.TextArea(introducation, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));


            if (GUILayout.Button("Github - FFramework"))
            {
                Application.OpenURL("https://github.com/yueh0607/FFramework");
            }
        }


        private Texture2D LoadTextureByIO(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            fs.Seek(0, SeekOrigin.Begin);
            byte[] bytes = new byte[fs.Length];
            try
            {
                fs.Read(bytes, 0, bytes.Length);

            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            fs.Close();

            int width = 2048;
            int height = 2048;
            Texture2D texture = new Texture2D(width, height);
            if (texture.LoadImage(bytes))
            {
                return texture;

            }
            else
            {
                return null;
            }
        }

    }
}

