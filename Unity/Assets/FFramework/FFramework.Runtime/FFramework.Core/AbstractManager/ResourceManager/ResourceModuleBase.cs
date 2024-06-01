namespace FFramework
{



    public abstract class ResourceModuleBase<T> where T : ResourceHandle
    {
        public abstract FTask<T> LoadAssetAsync(string assetPath);


    }
}
