namespace FFramework.Serialization
{
    public class PropertyOrder : System.Attribute
    {
        public int Order { get; private set; }

        public PropertyOrder(int order)
        {
            Order = order;
        }
    }
}
