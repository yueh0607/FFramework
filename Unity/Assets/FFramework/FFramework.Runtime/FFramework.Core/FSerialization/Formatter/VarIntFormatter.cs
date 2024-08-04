namespace FFramework.Serialization.Binary
{
    public class ByteFormatter : FSerializationFormatter<byte>
    {
        public override void Deserialize(ref DynamicSequence sequence, ref byte value)
        {
            value = VarIntReadWriteHelper.ReadByte(ref sequence);
        }

        public override void Serialize(ref DynamicSequence sequence, ref byte value)
        {
            VarIntReadWriteHelper.Write(ref sequence, value);
        }
    }

    public class SByteFormatter : FSerializationFormatter<sbyte>
    {
        public override void Deserialize(ref DynamicSequence sequence, ref sbyte value)
        {
            value = VarIntReadWriteHelper.ReadSByte(ref sequence);
        }

        public override void Serialize(ref DynamicSequence sequence, ref sbyte value)
        {
            VarIntReadWriteHelper.Write(ref sequence, value);
        }
    }

    public class ShortFormatter : FSerializationFormatter<short>
    {
        public override void Deserialize(ref DynamicSequence sequence, ref short value)
        {
            value = VarIntReadWriteHelper.ReadInt16(ref sequence);
        }

        public override void Serialize(ref DynamicSequence sequence, ref short value)
        {
            VarIntReadWriteHelper.Write(ref sequence, value);
        }
    }

    public class UShortFormatter : FSerializationFormatter<ushort>
    {
        public override void Deserialize(ref DynamicSequence sequence, ref ushort value)
        {
            value = VarIntReadWriteHelper.ReadUInt16(ref sequence);
        }

        public override void Serialize(ref DynamicSequence sequence, ref ushort value)
        {
            VarIntReadWriteHelper.Write(ref sequence, value);
        }
    }

    public class IntFormatter : FSerializationFormatter<int>
    {
        public override void Deserialize(ref DynamicSequence sequence, ref int value)
        {
            value = VarIntReadWriteHelper.ReadInt32(ref sequence);
        }

        public override void Serialize(ref DynamicSequence sequence, ref int value)
        {
            VarIntReadWriteHelper.Write(ref sequence, value);
        }
    }

    public class UIntFormatter : FSerializationFormatter<uint>
    {
        public override void Deserialize(ref DynamicSequence sequence, ref uint value)
        {
            value = VarIntReadWriteHelper.ReadUInt32(ref sequence);
        }

        public override void Serialize(ref DynamicSequence sequence, ref uint value)
        {
            VarIntReadWriteHelper.Write(ref sequence, value);
        }
    }

    public class LongFormatter : FSerializationFormatter<long>
    {
        public override void Deserialize(ref DynamicSequence sequence, ref long value)
        {
            value = VarIntReadWriteHelper.ReadInt64(ref sequence);
        }

        public override void Serialize(ref DynamicSequence sequence, ref long value)
        {
            VarIntReadWriteHelper.Write(ref sequence, value);
        }
    }

    public class ULongFormatter : FSerializationFormatter<ulong>
    {
        public override void Deserialize(ref DynamicSequence sequence, ref ulong value)
        {
            value = VarIntReadWriteHelper.ReadUInt64(ref sequence);
        }

        public override void Serialize(ref DynamicSequence sequence, ref ulong value)
        {
            VarIntReadWriteHelper.Write(ref sequence, value);
        }
    }
}
