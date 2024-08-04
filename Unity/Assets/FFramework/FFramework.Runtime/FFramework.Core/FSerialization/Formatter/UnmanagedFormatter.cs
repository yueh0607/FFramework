namespace FFramework.Serialization.Binary
{
    public class UnmanagedFormatter<T> : FSerializationFormatter<T> where T : unmanaged
    {
        public override void Deserialize(ref DynamicSequence sequence, ref T value)
        {
            value =  ReadWiteHelper.Read<T>(ref sequence);
        }

        public override void Serialize(ref DynamicSequence sequence, ref T value)
        {
            ReadWiteHelper.Write<T>(ref sequence, value);
        }
    }
}
