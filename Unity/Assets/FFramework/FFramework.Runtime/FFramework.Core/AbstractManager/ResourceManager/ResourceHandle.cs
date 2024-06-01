namespace FFramework
{
    public abstract class ResourceHandle : FDispoableUnit
    {
      
        public abstract void SetHandle(object handle);
        public abstract object GetHandle();

        public static T AllocateHandle<T, K>() where T : ResourceHandle where K : IPoolable<T>, new()
        {
            return Envirment.Current.GetModule<PoolModule>().Get<T, K>();
        }
        public static void ReleaseHandle<T, K>(T handle) where T : ResourceHandle where K : IPoolable<T>, new()
        {
            Envirment.Current.GetModule<PoolModule>().Set<T, K>(handle);
        }

    }
}
