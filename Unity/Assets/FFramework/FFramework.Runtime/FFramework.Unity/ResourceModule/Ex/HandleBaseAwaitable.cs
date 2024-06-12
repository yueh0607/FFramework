using YooAsset;

namespace FFramework
{
    public static partial class YooAssetAwaitable
    {
        public static IFTaskAwaiter GetAwaiter(this HandleBase handle)
        {
            return FTask.WaitUntil(() => handle.IsDone).GetAwaiter();
        }
    }
}
