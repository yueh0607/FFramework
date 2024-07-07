namespace FFramework
{
    public interface IModule
    {
        void OnCreate(object moduleParameter);

        void OnDestroy();

    }

}
