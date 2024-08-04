namespace FFramework.Serialization.Binary
{
    public interface IFSerializationFormatter
    {

    }
    public abstract class FSerializationFormatter<T> : IFSerializationFormatter
    {
        public abstract void Serialize(ref DynamicSequence sequence,ref T value);

        public abstract void Deserialize(ref DynamicSequence sequence,ref T value);
    }
}
