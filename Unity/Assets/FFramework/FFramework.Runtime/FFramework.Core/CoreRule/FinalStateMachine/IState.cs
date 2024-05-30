namespace FFramework
{
    public interface IState : IUpdate
    {
        void OnEnter();

        void OnExit();

        void OnCreate();

        void OnDestroy();
    }
}
