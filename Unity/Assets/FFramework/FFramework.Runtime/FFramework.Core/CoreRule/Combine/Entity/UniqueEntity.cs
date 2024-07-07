namespace FFramework
{
    public class UniqueEntity<Key, T> : Entity where T : IUniqueBranch<Key>
    {
        public T Branch { get; internal set; }
}
}
