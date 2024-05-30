namespace FFramework
{
    public interface IUpdate : ISendEvent<float>
    {
        void Update(float deltaTime);
    }
}
