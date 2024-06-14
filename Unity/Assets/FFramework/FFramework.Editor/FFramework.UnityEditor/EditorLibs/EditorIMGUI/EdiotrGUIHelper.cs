using UnityEditor;
using UnityEngine;

namespace FFramework.Utils.Editor
{
    public static class EditorGUIHelper
    {
        public static void DragPath(Rect dropAreaRect,ref string path)
        {
            if (dropAreaRect.Contains(Event.current.mousePosition))
            {
                // 路径拖拽
                if (Event.current.type == EventType.DragUpdated)
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    Event.current.Use();
                }
                else if (Event.current.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    foreach (string draggedPath in DragAndDrop.paths)
                    {
                        path = draggedPath;
                        break;
                    }
                    Event.current.Use();
                }

            }
        }
    }
}