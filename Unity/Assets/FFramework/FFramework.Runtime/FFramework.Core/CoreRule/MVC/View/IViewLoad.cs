namespace FFramework
{
    public interface IViewLoad<T> : ISendEvent<T> where T : struct
    {
        void OnLoad(T loadParameters);
    }
}
