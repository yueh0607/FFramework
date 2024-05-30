
namespace FFramework
{
    public abstract class AbstractState : FUnit, IState
    {
        public abstract void OnCreate();
        public abstract void OnDestroy();
        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void Update(float deltaTime);
    }
}
