namespace FFramework
{
    public class Singletion<T> where T : Singletion<T>
    {
        protected Singletion() { }

        public static T Instance => SingletonProperty<T>.Instance;

        public static void Unload() => SingletonProperty<T>.Unload();
    }
}
