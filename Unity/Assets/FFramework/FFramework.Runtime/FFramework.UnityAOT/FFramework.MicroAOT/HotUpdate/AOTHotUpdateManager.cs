using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using YooAsset;

namespace FFramework.MicroAOT
{
    public class AOTHotUpdateManager : AOTSingleton<AOTHotUpdateManager>
    {

        /// <summary>
        /// 补充元数据，应该在任何热更代码运行之前被完成
        /// </summary>
        /// <returns></returns>

        public IEnumerator PatchMetaData(bool isSimulated)
        {
            if (!isSimulated)
            {
                var infos = AOTResourceManager.Instance.GameLogicPackage.GetAssetInfos(AOTStartInfoManager.PatchMetaDataDllTag);

                List<RawFileHandle> handles = new List<RawFileHandle>();
                foreach (var assetInfo in infos)
                {
                    handles.Add(AOTResourceManager.Instance.GameLogicPackage.LoadRawFileAsync(assetInfo));
                }

                foreach (var handle in handles)
                {
                    yield return handle;
                    HybridCLR.RuntimeApi.LoadMetadataForAOTAssembly(handle.GetRawFileData(), HybridCLR.HomologousImageMode.SuperSet);
                }
            }
            else
            {
                //do nothing
            }

        }

        /// <summary>
        /// 加载热更程序集，调用每个程序集内的入口函数
        /// </summary>
        /// <returns></returns>
        public IEnumerator LoadHotUpdateAssemblies(bool isSimulated)
        {
            List<ValueTuple<int, MethodInfo>> m_Calls = new List<(int, MethodInfo)>();
            void InvokeEntry(Assembly ass, List<ValueTuple<int, MethodInfo>> callList)
            {
                var entryType = ass.GetType(AOTStartInfoManager.HotUpdateEntryClass, false);
                if (entryType == null) return;
                var method = entryType.GetMethod(AOTStartInfoManager.HotUpdateEntryMethod, BindingFlags.Static | BindingFlags.Public);
                if (method == null) return;

                var att = method.GetCustomAttribute<EntryPriorityAttribute>();
                callList.Add((att == null ? 0 : att.Priority, method));
            }

            void InvokeByPriority(List<ValueTuple<int, MethodInfo>> callList)
            {
                callList.Sort((x, y) => y.Item1 - x.Item1);
                foreach (var callItem in callList)
                {
                    _ = callItem.Item2.Invoke(null, null);
                }
            }

            if (!isSimulated)
            {
                var infos = AOTResourceManager.Instance.GameLogicPackage.GetAssetInfos(AOTStartInfoManager.HotUpdateDllTag);

                List<RawFileHandle> handles = new List<RawFileHandle>();

                foreach (var assetInfo in infos)
                {
                    handles.Add(AOTResourceManager.Instance.GameLogicPackage.LoadRawFileAsync(assetInfo));
                }
                Assembly[] hotUpdateAssemblies = new Assembly[handles.Count];
                foreach (var handle in handles)
                {
                    yield return handle;
                }


                for (int i = 0; i < hotUpdateAssemblies.Length; i++)
                {
                    hotUpdateAssemblies[i] = Assembly.Load(handles[i].GetRawFileData());

                    InvokeEntry(hotUpdateAssemblies[i], m_Calls);

                }
                InvokeByPriority(m_Calls);
            }
            else
            {

                var method = (Assembly.Load("FFramework.UnityEditor")?.GetType("FFramework.HotFix.Editor.HotFixTool", true)
                    ?.GetMethod("GetHotUpdateAssemblyNames", BindingFlags.Static | BindingFlags.Public)) ?? throw new InvalidProgramException("FFramework cannot find a method to get hot update assemblies name");

                List<string> result = (List<string>)method.Invoke(null, null);

                List<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies()
                    .Where((x) =>result.Contains(x.GetName().Name))
                    .ToList();

                foreach (var ass in assemblies)
                    InvokeEntry(ass, m_Calls);
                InvokeByPriority(m_Calls);

            }
        }

    }
}
