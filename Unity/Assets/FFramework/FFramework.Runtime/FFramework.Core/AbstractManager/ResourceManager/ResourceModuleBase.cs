namespace FFramework
{


    public abstract class ResourceModuleBase<AssetHandleType,AssetWhere> where AssetHandleType : ResourceHandle
    {
        public abstract AssetHandleType LoadAssetAsync<AssetType>(string assetPath) where AssetType:AssetWhere ; 

        public abstract AssetHandleType LoadAssetSync<AssetType>(string assetPath) where AssetType : AssetWhere;

     


    }
}
