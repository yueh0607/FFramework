namespace FFramework.Serialization.Binary
{
    public class ByteCollectionFormatter : FSerializationFormatter<byte[]>
    {
        public override void Deserialize(ref DynamicSequence sequence, ref byte[] value)
        {
            value = VarIntCollectionReadWriteHelper.ReadByteCollection(ref sequence);
        }

        public override void Serialize(ref DynamicSequence sequence, ref byte[] value)
        {
            VarIntReadWriteHelper.Write(ref sequence, value.Length);
            foreach (var item in value)
            {
                VarIntReadWriteHelper.Write(ref sequence, item);
            }
        }
    }

    public class SByteCollectionFormatter : FSerializationFormatter<sbyte[]>
    {
        public override void Deserialize(ref DynamicSequence sequence, ref sbyte[] value)
        {
            int length = VarIntReadWriteHelper.ReadInt32(ref sequence);
            value = new sbyte[length];
            for (int i = 0; i < length; i++)
            {
                value[i] = (sbyte)VarIntReadWriteHelper.ReadByte(ref sequence);
            }
        }

        public override void Serialize(ref DynamicSequence sequence, ref sbyte[] value)
        {
            VarIntReadWriteHelper.Write(ref sequence, value.Length);
            foreach (var item in value)
            {
                VarIntReadWriteHelper.Write(ref sequence, (byte)item);
            }
        }
    }

    public class ShortCollectionFormatter : FSerializationFormatter<short[]>
    {
        public override void Deserialize(ref DynamicSequence sequence, ref short[] value)
        {
            int length = VarIntReadWriteHelper.ReadInt32(ref sequence);
            value = new short[length];
            for (int i = 0; i < length; i++)
            {
                value[i] = VarIntReadWriteHelper.ReadInt16(ref sequence);
            }
        }

        public override void Serialize(ref DynamicSequence sequence, ref short[] value)
        {
            VarIntReadWriteHelper.Write(ref sequence, value.Length);
            foreach (var item in value)
            {
                VarIntReadWriteHelper.Write(ref sequence, item);
            }
        }
    }
    public class UShortCollectionFormatter : FSerializationFormatter<ushort[]>
    {
        public override void Deserialize(ref DynamicSequence sequence, ref ushort[] value)
        {
            int length = VarIntReadWriteHelper.ReadInt32(ref sequence);
            value = new ushort[length];
            for (int i = 0; i < length; i++)
            {
                value[i] = VarIntReadWriteHelper.ReadUInt16(ref sequence);
            }
        }

        public override void Serialize(ref DynamicSequence sequence, ref ushort[] value)
        {
            VarIntReadWriteHelper.Write(ref sequence, value.Length);
            foreach (var item in value)
            {
                VarIntReadWriteHelper.Write(ref sequence, item);
            }
        }
    }

    public class IntCollectionFormatter : FSerializationFormatter<int[]>
    {
        public override void Deserialize(ref DynamicSequence sequence, ref int[] value)
        {
            int length = VarIntReadWriteHelper.ReadInt32(ref sequence);
            value = new int[length];
            for (int i = 0; i < length; i++)
            {
                value[i] = VarIntReadWriteHelper.ReadInt32(ref sequence);
            }
        }

        public override void Serialize(ref DynamicSequence sequence, ref int[] value)
        {
            VarIntReadWriteHelper.Write(ref sequence, value.Length);
            foreach (var item in value)
            {
                VarIntReadWriteHelper.Write(ref sequence, item);
            }
        }
    }
    public class UIntCollectionFormatter : FSerializationFormatter<uint[]>
    {
        public override void Deserialize(ref DynamicSequence sequence, ref uint[] value)
        {
            int length = VarIntReadWriteHelper.ReadInt32(ref sequence);
            value = new uint[length];
            for (int i = 0; i < length; i++)
            {
                value[i] = VarIntReadWriteHelper.ReadUInt32(ref sequence);
            }
        }

        public override void Serialize(ref DynamicSequence sequence, ref uint[] value)
        {
            VarIntReadWriteHelper.Write(ref sequence, value.Length);
            foreach (var item in value)
            {
                VarIntReadWriteHelper.Write(ref sequence, item);
            }
        }
    }

    public class LongCollectionFormatter : FSerializationFormatter<long[]>
    {
        public override void Deserialize(ref DynamicSequence sequence, ref long[] value)
        {
            int length = VarIntReadWriteHelper.ReadInt32(ref sequence);
            value = new long[length];
            for (int i = 0; i < length; i++)
            {
                value[i] = VarIntReadWriteHelper.ReadInt64(ref sequence);
            }
        }

        public override void Serialize(ref DynamicSequence sequence, ref long[] value)
        {
            VarIntReadWriteHelper.Write(ref sequence, value.Length);
            foreach (var item in value)
            {
                VarIntReadWriteHelper.Write(ref sequence, item);
            }
        }
    }
    public class ULongCollectionFormatter : FSerializationFormatter<ulong[]>
    {
        public override void Deserialize(ref DynamicSequence sequence, ref ulong[] value)
        {
            int length = VarIntReadWriteHelper.ReadInt32(ref sequence);
            value = new ulong[length];
            for (int i = 0; i < length; i++)
            {
                value[i] = VarIntReadWriteHelper.ReadUInt64(ref sequence);
            }
        }

        public override void Serialize(ref DynamicSequence sequence, ref ulong[] value)
        {
            VarIntReadWriteHelper.Write(ref sequence, value.Length);
            foreach (var item in value)
            {
                VarIntReadWriteHelper.Write(ref sequence, item);
            }
        }
    }
}
