using System.Text;


namespace FFramework
{
    public static partial class BuiltInPoolables
    {
        public class StringBuilderPoolable : IPoolable<StringBuilder>
        {
            int IPoolable.Capacity => 1000;

            StringBuilder IPoolable<StringBuilder>.OnCreate()
            {
                return new StringBuilder();
            }

            void IPoolable<StringBuilder>.OnDestroy(StringBuilder obj)
            {

            }

            void IPoolable<StringBuilder>.OnGet(StringBuilder obj)
            {

            }

            void IPoolable<StringBuilder>.OnSet(StringBuilder obj)
            {
                obj.Clear();
            }
        }
    }
}