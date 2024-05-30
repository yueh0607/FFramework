namespace FFramework
{
    public enum FTaskStatus
    {
        //悬而未决
        Pending,
        //挂起
        Suspending,
        //取消
        Canceled,
        //失败
        Failed,
        //成功
        Succeed
    }

    public static class FTaskStatusExtension
    {
        public static bool IsFinished(this FTaskStatus status)
        {
            return status == FTaskStatus.Canceled || status == FTaskStatus.Failed || status == FTaskStatus.Succeed;
        }
        public static bool IsRunning(this FTaskStatus status)
        {
            return status == FTaskStatus.Pending || status == FTaskStatus.Suspending;
        }

        public static bool IsSuspending(this FTaskStatus status)
        {
            return status == FTaskStatus.Suspending;
        }

    }
}