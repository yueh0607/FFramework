﻿using System;
using System.Collections.Generic;
namespace FFramework
{

    public static partial class DictionaryExtensions
    {
        #region Dictionary
        /// <summary>
        /// 获取字典值，如果不存在则通过new创建并添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static K GetValueOrAddNew<T, K>(this IDictionary<T, K> dictionary, T key) where K : new()
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, new K());
            }
            return dictionary[key];
        }
        /// <summary>
        /// 获取或者添加传入的默认值,onCreate不建议传入一次性Func，否则会产生GC
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="defaultVaue"></param>
        /// <returns></returns>
        public static K GetValueOrAddDefault<T, K>(this IDictionary<T, K> dictionary, T key, Func<K> onCreate)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, onCreate());
            }
            return dictionary[key];
        }
        /// <summary>
        /// 获取或添加指定默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static K GetValueOrAddDefault<T, K>(this IDictionary<T, K> dictionary, T key, K defaultValue = default)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, defaultValue);
            }
            return dictionary[key];
        }

        /// <summary>
        /// 尝试从字典内移除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool TryRemove<T, K>(this Dictionary<T, K> dictionary, T key)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary.Remove(key);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 尝试从字典移除并释放
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool TryRemoveAndDispose<T, K>(this Dictionary<T, K> dictionary, T key) where K : IDisposable
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key].Dispose();
                dictionary.Remove(key);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 从字典移除并释放
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        public static void RemoveAndDispose<T, K>(this Dictionary<T, K> dictionary, T key) where K : IDisposable
        {
            if (!dictionary.TryRemoveAndDispose(key)) throw new InvalidOperationException("No such key in hashSet!if possible,please insteading of TryRemoveAndDispose()");
        }
        /// <summary>
        /// 清空并释放字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="dictionary"></param>
        public static void ClearAndDispose<T, K>(this Dictionary<T, K> dictionary) where K : IDisposable
        {
            foreach (var kvp in dictionary)
            {
                kvp.Value.Dispose();
            }
            dictionary.Clear();
        }

        /// <summary>
        /// 设置全部值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="defaultValue"></param>
        public static void SetAllValue<T, K>(this IDictionary<T, K> dictionary, K defaultValue)
        {
            foreach (var x in dictionary.Keys)
            {
                dictionary[x] = defaultValue;
            }
        }

        /// <summary>
        /// 添加或者设置值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddOrSet<T, K>(this IDictionary<T, K> dictionary, T key, K value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            dictionary.Add(key, value);
        }
        #endregion
    }
}
