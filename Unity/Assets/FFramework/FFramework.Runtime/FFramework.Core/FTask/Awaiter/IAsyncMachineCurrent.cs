namespace FFramework
{
    public interface IAsyncMachineCurrent 
    {
        public IFTaskAwaiter CurrentAwaiter { get; set; }
    }
}
