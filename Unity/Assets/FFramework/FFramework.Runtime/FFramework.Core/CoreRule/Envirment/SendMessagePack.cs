namespace FFramework
{
    public class SendMessagePack<T> : IMessagePack
    {
        public float deltaTime;

        IUpdate listener;

        public void Invoke()
        {
            listener.Send<IUpdate>(deltaTime);
        }
    }
}
