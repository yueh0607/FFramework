namespace FFramework
{
    public interface IFixedUpdate : ISendEvent<float>
    {
        void FixedUpdate(float deltaTime);
    }
}
