using System;
using System.Collections.Generic;
using YooAsset;

namespace FFramework
{
    [ModuleStatic("FResource")]
    public class UnityResourceModule : IModule
    {
        public UnityAssetHandle LoadAssetAsync<T>(string assetPath) where T : UnityEngine.Object
        {
            UnityAssetHandle handle =
                Envirment.Current.GetModule<PoolModule>().Get<UnityAssetHandle, UnityAssetHandle.Poolable>();

            AssetHandle yooHandle = YooAssets.LoadAssetAsync<T>(assetPath);

            handle.SetHandle(yooHandle);

            return handle;
        }

        public string[] LoadAssetAsyncByTag(string tag)
        {
            var assetInfos = YooAssets.GetAssetInfos(tag);
            string[] paths = new string[assetInfos.Length];
            for (int i = 0; i < paths.Length; i++)
                paths[i] = assetInfos[i].AssetPath;
            return paths;
        }

        /// <summary>
        /// 加载原生资源（bytes、text）
        /// </summary>
        /// <typeparam name="AssetType"></typeparam>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public UnityRawFileHandle LoadRawFileAsync(string assetPath)
        {
            UnityRawFileHandle handle =
                Envirment.Current.GetModule<PoolModule>().Get<UnityRawFileHandle, UnityRawFileHandle.Poolable>();

            RawFileHandle yooHandle = YooAssets.LoadRawFileAsync(assetPath);
            handle.SetHandle(yooHandle);
            return handle;
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <typeparam name="AssetType"></typeparam>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public UnitySceneHandle LoadSceneAsync(string assetPath)
        {
            UnitySceneHandle handle =
               Envirment.Current.GetModule<PoolModule>().Get<UnitySceneHandle, UnitySceneHandle.Poolable>();

            SceneHandle yooHandle = YooAssets.LoadSceneAsync(assetPath);
            handle.SetHandle(yooHandle);
            return handle;
        }


        /// <summary>
        /// 加载子资源(SpriteAltas)
        /// </summary>
        /// <typeparam name="AssetType"></typeparam>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public UnitySubAssetHandle LoadSubAssetsAsync<AssetType>(string assetPath) where AssetType : UnityEngine.Object
        {
            UnitySubAssetHandle handle =
              Envirment.Current.GetModule<PoolModule>().Get<UnitySubAssetHandle, UnitySubAssetHandle.Poolable>();

            SubAssetsHandle yooHandle = YooAssets.LoadSubAssetsAsync<AssetType>(assetPath);
            handle.SetHandle(yooHandle);
            return handle;
        }


        void IModule.OnCreate(object moduleParameter)
        {

        }

        void IModule.OnDestroy()
        {

        }
    }
}
