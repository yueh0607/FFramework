namespace FFramework
{
    internal class FTaskConst
    {
        //非泛型FTask池容量
        public const int NOT_GENERIC_FTASK_PER_TYPE_POOL_CAPACITY = 1000;
        //泛型FTask池容量
        public const int GENERIC_FTASK_PER_TYPE_POOL_CAPACITY = 500;
        //TokenCatch容量
        public const int TOKEN_CATCH_POOL_CAPACITY = 1000;
        //令牌池容量
        public const int TOKEN_POOL_CAPACITY = 1000;



        //任务已完成
        public const string FTASK_ALREADY_FINISHED_MESSAGE = "FTask is already finished";
        //不能重置运行中的Awaiter
        public const string FTASK_NOT_FINISHED_MESSAGE = "FTask is pending";

    }
}
