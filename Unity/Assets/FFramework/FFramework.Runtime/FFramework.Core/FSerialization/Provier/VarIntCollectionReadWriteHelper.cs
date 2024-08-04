using System.Collections.Generic;
using FFramework.Serialization.Binary;

namespace FFramework
{
    /// <summary>
    /// 变长数组写入器
    /// </summary>
    public static class VarIntCollectionReadWriteHelper
    {
        public static void WriteVarIntCollection(ref DynamicSequence sequence, byte[] value)
        {
            //写入长度
            VarIntReadWriteHelper.Write(ref sequence, value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                VarIntReadWriteHelper.Write(ref sequence, value[i]);
            }
        }
        public static void WriteVarIntCollection(ref DynamicSequence sequence, ICollection<byte> value)
        {
            //写入长度
            VarIntReadWriteHelper.Write(ref sequence, value.Count);
            foreach (var item in value)
            {
                VarIntReadWriteHelper.Write(ref sequence, item);
            }
        }
        public static void WriteVarIntCollection(ref DynamicSequence sequence, sbyte[] value)
        {
            //写入长度
            VarIntReadWriteHelper.Write(ref sequence, value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                VarIntReadWriteHelper.Write(ref sequence, value[i]);
            }
        }
        public static void WriteVarIntCollection(ref DynamicSequence sequence, ICollection<sbyte> value)
        {
            //写入s长度
            VarIntReadWriteHelper.Write(ref sequence, value.Count);
            foreach (var item in value)
            {
                VarIntReadWriteHelper.Write(ref sequence, item);
            }
        }
        public static void WriteVarIntCollection(ref DynamicSequence sequence, short[] value)
        {
            //写入长度
            VarIntReadWriteHelper.Write(ref sequence, value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                VarIntReadWriteHelper.Write(ref sequence, value[i]);
            }
        }
        public static void WriteVarIntCollection(ref DynamicSequence sequence, ICollection<short> value)
        {
            //写入长度
            VarIntReadWriteHelper.Write(ref sequence, value.Count);
            foreach (var item in value)
            {
                VarIntReadWriteHelper.Write(ref sequence, item);
            }
        }
        public static void WriteVarIntCollection(ref DynamicSequence sequence, ushort[] value)
        {
            //写入长度
            VarIntReadWriteHelper.Write(ref sequence, value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                VarIntReadWriteHelper.Write(ref sequence, value[i]);
            }
        }
        public static void WriteVarIntCollection(ref DynamicSequence sequence, ICollection<ushort> value)
        {
            //写入长度
            VarIntReadWriteHelper.Write(ref sequence, value.Count);
            foreach (var item in value)
            {
                VarIntReadWriteHelper.Write(ref sequence, item);
            }
        }
        public static void WriteVarIntCollection(ref DynamicSequence sequence, int[] value)
        {
            //写入长度
            VarIntReadWriteHelper.Write(ref sequence, value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                VarIntReadWriteHelper.Write(ref sequence, value[i]);
            }
        }
        public static void WriteVarIntCollection(ref DynamicSequence sequence, ICollection<int> value)
        {
            //写入长度
            VarIntReadWriteHelper.Write(ref sequence, value.Count);
            foreach (var item in value)
            {
                VarIntReadWriteHelper.Write(ref sequence, item);
            }
        }
        public static void WriteVarIntCollection(ref DynamicSequence sequence, uint[] value)
        {
            //写入长度
            VarIntReadWriteHelper.Write(ref sequence, value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                VarIntReadWriteHelper.Write(ref sequence, value[i]);
            }
        }
        public static void WriteVarIntCollection(ref DynamicSequence sequence, ICollection<uint> value)
        {
            //写入长度
            VarIntReadWriteHelper.Write(ref sequence, value.Count);
            foreach (var item in value)
            {
                VarIntReadWriteHelper.Write(ref sequence, item);
            }
        }
        public static void WriteVarIntCollection(ref DynamicSequence sequence, long[] value)
        {
            //写入长度
            VarIntReadWriteHelper.Write(ref sequence, value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                VarIntReadWriteHelper.Write(ref sequence, value[i]);
            }
        }
        public static void WriteVarIntCollection(ref DynamicSequence sequence, ICollection<long> value)
        {
            //写入长度
            VarIntReadWriteHelper.Write(ref sequence, value.Count);
            foreach (var item in value)
            {
                VarIntReadWriteHelper.Write(ref sequence, item);
            }
        }
        public static void WriteVarIntCollection(ref DynamicSequence sequence, ulong[] value)
        {
            //写入长度
            VarIntReadWriteHelper.Write(ref sequence, value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                VarIntReadWriteHelper.Write(ref sequence, value[i]);
            }
        }
        public static void WriteVarIntCollection(ref DynamicSequence sequence, ICollection<ulong> value)
        {
            //写入长度
            VarIntReadWriteHelper.Write(ref sequence, value.Count);
            foreach (var item in value)
            {
                VarIntReadWriteHelper.Write(ref sequence, item);
            }
        }

        public static ArrayWrapper<byte> ReadByteCollection(ref DynamicSequence sequence)
        {
            int length = VarIntReadWriteHelper.ReadInt32(ref sequence);
            var array = Envirment.Current.GetModule<PoolModule>().Get<ArrayWrapper<byte>, ArrayWrapper<byte>.Poolable>();
            array.Preserve(length);
            for (int i = 0; i < length; i++)
            {
                array[i] = VarIntReadWriteHelper.ReadByte(ref sequence);
            }
            return array;
        }

        public static void ReadByteCollection(ref DynamicSequence sequence, ICollection<byte> collection)
        {
            int length = VarIntReadWriteHelper.ReadInt32(ref sequence);
            for (int i = 0; i < length; i++)
            {
                collection.Add(VarIntReadWriteHelper.ReadByte(ref sequence));
            }
        }

        public static ArrayWrapper<sbyte> ReadSByteCollection(ref DynamicSequence sequence)
        {
            int length = VarIntReadWriteHelper.ReadInt32(ref sequence);
            var array = Envirment.Current.GetModule<PoolModule>().Get<ArrayWrapper<sbyte>, ArrayWrapper<sbyte>.Poolable>();
            array.Preserve(length);
            for (int i = 0; i < length; i++)
            {
                array[i] = (sbyte)VarIntReadWriteHelper.ReadByte(ref sequence);
            }
            return array;
        }

        public static void ReadSByteCollection(ref DynamicSequence sequence, ICollection<sbyte> collection)
        {
            int length = VarIntReadWriteHelper.ReadInt32(ref sequence);
            for (int i = 0; i < length; i++)
            {
                collection.Add((sbyte)VarIntReadWriteHelper.ReadByte(ref sequence));
            }
        }

        public static ArrayWrapper<short> ReadShortCollection(ref DynamicSequence sequence)
        {
            int length = VarIntReadWriteHelper.ReadInt32(ref sequence);
            var array = Envirment.Current.GetModule<PoolModule>().Get<ArrayWrapper<short>, ArrayWrapper<short>.Poolable>();
            array.Preserve(length);
            for (int i = 0; i < length; i++)
            {
                array[i] = VarIntReadWriteHelper.ReadInt16(ref sequence);
            }
            return array;
        }

        public static void ReadShortCollection(ref DynamicSequence sequence, ICollection<short> collection)
        {
            int length = VarIntReadWriteHelper.ReadInt32(ref sequence);
            for (int i = 0; i < length; i++)
            {
                collection.Add(VarIntReadWriteHelper.ReadInt16(ref sequence));
            }
        }

        public static ArrayWrapper<ushort> ReadUShortCollection(ref DynamicSequence sequence)
        {
            int length = VarIntReadWriteHelper.ReadInt32(ref sequence);
            var array = Envirment.Current.GetModule<PoolModule>().Get<ArrayWrapper<ushort>, ArrayWrapper<ushort>.Poolable>();
            array.Preserve(length);
            for (int i = 0; i < length; i++)
            {
                array[i] = VarIntReadWriteHelper.ReadUInt16(ref sequence);
            }
            return array;
        }

        public static void ReadUShortCollection(ref DynamicSequence sequence, ICollection<ushort> collection)
        {
            int length = VarIntReadWriteHelper.ReadInt32(ref sequence);
            for (int i = 0; i < length; i++)
            {
                collection.Add(VarIntReadWriteHelper.ReadUInt16(ref sequence));
            }
        }

        public static ArrayWrapper<int> ReadIntCollection(ref DynamicSequence sequence)
        {
            int length = VarIntReadWriteHelper.ReadInt32(ref sequence);
            var array = Envirment.Current.GetModule<PoolModule>().Get<ArrayWrapper<int>, ArrayWrapper<int>.Poolable>();
            array.Preserve(length);
            for (int i = 0; i < length; i++)
            {
                array[i] = VarIntReadWriteHelper.ReadInt32(ref sequence);
            }
            return array;
        }

        public static void ReadIntCollection(ref DynamicSequence sequence, ICollection<int> collection)
        {
            int length = VarIntReadWriteHelper.ReadInt32(ref sequence);
            for (int i = 0; i < length; i++)
            {
                collection.Add(VarIntReadWriteHelper.ReadInt32(ref sequence));
            }
        }

        public static ArrayWrapper<uint> ReadUIntCollection(ref DynamicSequence sequence)
        {
            int length = VarIntReadWriteHelper.ReadInt32(ref sequence);
            var array = Envirment.Current.GetModule<PoolModule>().Get<ArrayWrapper<uint>, ArrayWrapper<uint>.Poolable>();
            array.Preserve(length);
            for (int i = 0; i < length; i++)
            {
                array[i] = VarIntReadWriteHelper.ReadUInt32(ref sequence);
            }
            return array;
        }

        public static void ReadUIntCollection(ref DynamicSequence sequence, ICollection<uint> collection)
        {
            int length = VarIntReadWriteHelper.ReadInt32(ref sequence);
            for (int i = 0; i < length; i++)
            {
                collection.Add(VarIntReadWriteHelper.ReadUInt32(ref sequence));
            }
        }

        public static ArrayWrapper<long> ReadLongCollection(ref DynamicSequence sequence)
        {
            int length = VarIntReadWriteHelper.ReadInt32(ref sequence);
            var array = Envirment.Current.GetModule<PoolModule>().Get<ArrayWrapper<long>, ArrayWrapper<long>.Poolable>();
            array.Preserve(length);
            for (int i = 0; i < length; i++)
            {
                array[i] = VarIntReadWriteHelper.ReadInt64(ref sequence);
            }
            return array;
        }

        public static void ReadLongCollection(ref DynamicSequence sequence, ICollection<long> collection)
        {
            int length = VarIntReadWriteHelper.ReadInt32(ref sequence);
            for (int i = 0; i < length; i++)
            {
                collection.Add(VarIntReadWriteHelper.ReadInt64(ref sequence));
            }
        }

        public static ArrayWrapper<ulong> ReadULongCollection(ref DynamicSequence sequence)
        {
            int length = VarIntReadWriteHelper.ReadInt32(ref sequence);
            var array = Envirment.Current.GetModule<PoolModule>().Get<ArrayWrapper<ulong>, ArrayWrapper<ulong>.Poolable>();
            array.Preserve(length);
            for (int i = 0; i < length; i++)
            {
                array[i] = VarIntReadWriteHelper.ReadUInt64(ref sequence);
            }
            return array;
        }

        public static void ReadULongCollection(ref DynamicSequence sequence, ICollection<ulong> collection)
        {
            int length = VarIntReadWriteHelper.ReadInt32(ref sequence);
            for (int i = 0; i < length; i++)
            {
                collection.Add(VarIntReadWriteHelper.ReadUInt64(ref sequence));
            }
        }
    }
}

