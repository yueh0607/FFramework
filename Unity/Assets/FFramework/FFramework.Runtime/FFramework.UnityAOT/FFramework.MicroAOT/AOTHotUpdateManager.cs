using System.Collections;
using System.Collections.Generic;
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

        public IEnumerator PatchMetaData()
        {
            var infos = AOTResourceManager.Instance.LoadAllAssetByTag(AOTStartInfoManager.PatchMetaDataDllTag);

            List<RawFileHandle> handles = new List<RawFileHandle>();
            foreach (var assetInfo in infos)
            {
                handles.Add(AOTResourceManager.Instance.LoadRawFileAsync(assetInfo));
            }

            foreach (var handle in handles)
            {
                yield return handle;
                HybridCLR.RuntimeApi.LoadMetadataForAOTAssembly(handle.GetRawFileData(), HybridCLR.HomologousImageMode.SuperSet);
            }
        }

        /// <summary>
        /// 加载热更程序集，调用每个程序集内的入口函数
        /// </summary>
        /// <returns></returns>
        public IEnumerator LoadHotUpdateAssemblies()
        {
            var infos = AOTResourceManager.Instance.LoadAllAssetByTag(AOTStartInfoManager.HotUpdateDllTag);

            List<RawFileHandle> handles = new List<RawFileHandle>();

            foreach (var assetInfo in infos)
            {
                handles.Add(AOTResourceManager.Instance.LoadRawFileAsync(assetInfo));
            }
            Assembly[] hotUpdateAssemblies = new Assembly[handles.Count];
            foreach (var handle in handles)
            {
                yield return handle;
            }


            for (int i = 0; i < hotUpdateAssemblies.Length; i++)
            {
                var entryType = hotUpdateAssemblies[i].GetType(AOTStartInfoManager.HotUpdateEntryClass, false);
                if (entryType == null) continue;
                var method = entryType.GetMethod(AOTStartInfoManager.HotUpdateEntryMethod, BindingFlags.Static | BindingFlags.Public);
                if (method == null) continue;
                _ = method.Invoke(null, null);
            }
        }

    }
}
