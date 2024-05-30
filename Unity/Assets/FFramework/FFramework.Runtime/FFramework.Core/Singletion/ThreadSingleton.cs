namespace FFramework
{
    /// <summary>
    /// 线程独立副本的单例，用更多的内存代价、数据不能共享来换取无锁的性能
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ThreadSingletion<T> where T : ThreadSingletion<T>
    {
        protected ThreadSingletion() { }

        public static T Instance => ThreadSingletonProperty<T>.Instance;

        public static void Unload() => ThreadSingletonProperty<T>.Unload();
    }
}
