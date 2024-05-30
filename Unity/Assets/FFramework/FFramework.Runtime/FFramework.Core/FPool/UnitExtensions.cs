namespace FFramework
{
    public static class UnitExtensions
    {
        public static void Recycle(this FUnit unit)
        {
            Envirment.Current.GetModule<PoolModule>().InternalSet(unit.GetType(), unit);
        }
    }
}
