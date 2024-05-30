namespace FFramework
{
    public interface ISucceedCallback 
    {
        void SetSucceed();
    }

    public interface ISucceedCallback<T>
    {
        void SetSucceed(T result);
    }
}
