using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFramework
{
    public static class TreeForeachHelper
    {
        /// <summary>
        /// 深度优先搜索
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="childrenGetter"></param>
        /// <param name="onArrive"></param>
        /// <returns></returns>
        public static int DepthFirstSearch<T>(T root, Func<T, T[]> childrenGetter, Action<T> onArrive)
        {
            if (root == null) return 0;
            var stack = new Stack<T>();
            stack.Push(root);
            int count = 0;

            while (stack.Count > 0)
            {
                var node = stack.Pop();
                onArrive(node);
                count++;
                var children = childrenGetter(node);
                if (children != null)
                {
                    for (int i = children.Length - 1; i >= 0; i--)
                    {
                        stack.Push(children[i]);
                    }
                }
            }
            return count;
        }
        /// <summary>
        /// 广度优先搜索
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="childrenGetter"></param>
        /// <param name="onArrive"></param>
        /// <returns></returns>
        public static int BreadthFirstSearch<T>(T root, Func<T, T[]> childrenGetter, Action<T> onArrive)
        {
            if (root == null) return 0;
            var queue = new Queue<T>();
            queue.Enqueue(root);
            int count = 0;

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                onArrive(node);
                count++;
                var children = childrenGetter(node);
                if (children != null)
                {
                    foreach (var child in children)
                    {
                        queue.Enqueue(child);
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// 先序遍历
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="childrenGetter"></param>
        /// <param name="onArrive"></param>
        /// <returns></returns>
        public static int PreorderTraversal<T>(T root, Func<T, T[]> childrenGetter, Action<T> onArrive)
        {
            return DepthFirstSearch(root, childrenGetter, onArrive);
        }

        /// <summary>
        /// 中序遍历
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="childrenGetter"></param>
        /// <param name="onArrive"></param>
        /// <returns></returns>
        public static int MiddleorderTraversal<T>(T root, Func<T, T[]> childrenGetter, Action<T> onArrive)
        {
            if (root == null) return 0;
            var stack = new Stack<(T, bool)>();
            stack.Push((root, false));
            int count = 0;

            while (stack.Count > 0)
            {
                var (node, visited) = stack.Pop();
                if (visited)
                {
                    onArrive(node);
                    count++;
                }
                else
                {
                    var children = childrenGetter(node);
                    if (children != null && children.Length > 0)
                    {
                        for (int i = children.Length - 1; i > 0; i--)
                        {
                            stack.Push((children[i], false));
                        }
                        stack.Push((node, true));
                        stack.Push((children[0], false));
                    }
                    else
                    {
                        onArrive(node);
                        count++;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// 后序遍历
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="childrenGetter"></param>
        /// <param name="onArrive"></param>
        /// <returns></returns>
        public static int PostorderTraversal<T>(T root, Func<T, T[]> childrenGetter, Action<T> onArrive)
        {
            if (root == null) return 0;
            var stack = new Stack<(T, bool)>();
            stack.Push((root, false));
            int count = 0;

            while (stack.Count > 0)
            {
                var (node, visited) = stack.Pop();
                if (visited)
                {
                    onArrive(node);
                    count++;
                }
                else
                {
                    var children = childrenGetter(node);
                    stack.Push((node, true));
                    if (children != null)
                    {
                        for (int i = children.Length - 1; i >= 0; i--)
                        {
                            stack.Push((children[i], false));
                        }
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// 层序遍历
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="childrenGetter"></param>
        /// <param name="onArrive"></param>
        /// <returns></returns>
        public static int LevelTraversal<T>(T root, Func<T, T[]> childrenGetter, Action<T> onArrive)
        {
            return BreadthFirstSearch(root, childrenGetter, onArrive);
        }

    }
}
