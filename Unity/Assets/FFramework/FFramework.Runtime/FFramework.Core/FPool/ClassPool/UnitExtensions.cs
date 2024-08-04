namespace FFramework
{
    public static class UnitExtensions
    {
        public static void Recycle<T>(this FPoolUnit<T> unit) where T : FPoolUnit<T>
        {
            Envirment.Current.GetModule<PoolModule>().InternalSet(unit.GetType(), unit);
        }
    }
}
