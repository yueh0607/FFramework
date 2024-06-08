namespace FFramework
{
    /// <summary>
    /// 用于指示一个Awaiter是同步等待器，同步等待器表现上虽然是异步，但是会被自动的瞬时完成
    /// </summary>
    public interface ISyncAwaiter : ISucceedCallback
    {
        
    }
}
