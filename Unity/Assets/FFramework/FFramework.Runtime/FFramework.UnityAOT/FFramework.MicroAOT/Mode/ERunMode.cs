namespace FFramework
{
    public enum ERunMode
    {
        /// <summary>
        /// 模拟模式，将会采用各种手段隔离打包时的耗时操作以及繁琐的操作，以达到无需操作就能快速运行的目的
        /// 并不一定是每个功能都比Real快，但是一定会更简单，部分功能实机会更快
        /// </summary>
        Simulated,

        /// <summary>
        /// 模拟真实环境下的逻辑
        /// </summary>
        Real
    }
}
