namespace FFramework
{
    public interface ITimerUpdate : ISendEvent<float>
    {
        void Update(float deltaTime);
    }
}
