namespace FFramework
{
    public interface IViewUnload<T> : ISendEvent<T> where T : struct
    {
        void OnUnload(T unloadParameters);
    }


}
