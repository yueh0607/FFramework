namespace FFramework
{
    public interface IPoolRecyclable 
    {
        
    }


    public static class PoolRecycableExtension
    {
        public static void Recycle<T>(this T obj) where T: class,IPoolRecyclable
        {
            Envirment.Current.GetModule<PoolModule>().Set<T>(obj);
        }
        public static void Recycle<T,K>(this T obj) where T : class, IPoolRecyclable where K : IPoolable<T>,new()
        {
            Envirment.Current.GetModule<PoolModule>().Set<T,K>(obj);
        }
    }
}
