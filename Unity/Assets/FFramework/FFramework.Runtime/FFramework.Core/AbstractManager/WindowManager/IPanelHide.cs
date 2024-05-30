namespace FFramework
{
    public interface IPanelHide<T> : ISendEvent<T> where T : struct
    {
        void OnHide(T hideParameters);
    }


}
