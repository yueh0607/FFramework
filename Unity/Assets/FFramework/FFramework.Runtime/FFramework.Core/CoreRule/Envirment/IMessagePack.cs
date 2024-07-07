namespace FFramework
{
    public interface IMessagePack : ISendEvent
    {
        /// <summary>
        /// 处理消息
        /// </summary>
        void Invoke();
    }
}
