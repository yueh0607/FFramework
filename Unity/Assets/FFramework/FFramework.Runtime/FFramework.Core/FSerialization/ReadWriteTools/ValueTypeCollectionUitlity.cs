using FFramework.Serialization;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace FFramework
{
    internal class ValueTypeCollectionUitlity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void WriteCollection<T>(byte* destination, ref int offset, ICollection<T> value) where T : struct
        {
            if (value != null)
            {
                int len = value.Count;
                SerializeSizeWarpper.WriteUnaligned(destination + offset,
                    SerializeSizeWarpper.ConvertInt(len));
                offset += SerializeSizeWarpper.LENGTH;
                var enumerator = value.GetEnumerator();
                while (enumerator.MoveNext())
                    ValueTypeUtility.Write(destination, ref offset, enumerator.Current);
            }
            else
            {
                SerializeSizeWarpper.WriteUnaligned(destination + offset, 0);
                offset += SerializeSizeWarpper.LENGTH;
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe T ReadCollection<T, T1>(byte* source, ref int offset) where T : ICollection<T1>, new() where T1 : struct
        {
            var arrayLength = SerializeSizeWarpper.ReadUnaligned(source + offset);
            offset += SerializeSizeWarpper.LENGTH;
            if (arrayLength > 0)
            {
                var value = new T();
                var newValue = new T1[arrayLength];
                void* baseAddress = Unsafe.AsPointer(ref newValue[0]);
                int count = arrayLength * Unsafe.SizeOf<T1>();
                Unsafe.CopyBlockUnaligned(baseAddress, source + offset, (uint)count);
                offset += count;
                for (int i = 0; i < arrayLength; i++)
                    value.Add(newValue[i]);
                return value;
            }
            return default;
        }
    }
}
