using YooAsset;

namespace FFramework
{
    public class UnityResourceModule : ResourceModuleBase<UnityAssetHandle, UnityEngine.Object>, IModule
    {
        public override UnityAssetHandle LoadAssetAsync<T>(string assetPath)
        {
            UnityAssetHandle handle =
                Envirment.Current.GetModule<PoolModule>().Get<UnityAssetHandle, UnityAssetHandle.Poolable>();

            AssetHandle yooHandle = YooAssets.LoadAssetAsync<T>(assetPath);
            handle.SetHandle(yooHandle);
      
            return handle;
        }

        public override UnityAssetHandle LoadAssetSync<T>(string assetPath)
        {
            UnityAssetHandle handle =
                Envirment.Current.GetModule<PoolModule>().Get<UnityAssetHandle, UnityAssetHandle.Poolable>();

            AssetHandle yooHandle = YooAssets.LoadAssetSync<T>(assetPath);
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
