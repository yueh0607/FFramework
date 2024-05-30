namespace FFramework
{
    public interface IRealTimeUpdate : ISendEvent<float>
    {
        void RealUpdate(float deltaTime);
    }
}
