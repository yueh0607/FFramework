using System.Runtime.CompilerServices;


namespace FFramework.Serialization
{
    internal static class ValueTypeArrayUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void Write<T>(byte* destination, ref int offset, T[] array) where T : struct
        {
            if (array != null)
            {
                void* baseAddress = Unsafe.AsPointer(ref array[0]);
                int arrayLength = array.Length;
                SerializeSizeWarpper.WriteUnaligned(destination + offset, 
                    SerializeSizeWarpper.ConvertInt(arrayLength));
                offset += SerializeSizeWarpper.LENGTH;
                int count = arrayLength * Unsafe.SizeOf<T>();
                Unsafe.CopyBlockUnaligned(destination + offset, baseAddress, (uint)count);
                offset += count;
            }
            else
            {
                SerializeSizeWarpper.WriteUnaligned(destination + offset,0);
                offset += SerializeSizeWarpper.LENGTH;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe T[] ReadArray<T>(byte* source, ref int offset) where T : struct
        {
            var arrayLength = SerializeSizeWarpper.ReadUnaligned(source + offset);
            offset += SerializeSizeWarpper.LENGTH;
            if (arrayLength > 0)
            {
                var resultArray = new T[arrayLength];
                void* baseAddress = Unsafe.AsPointer(ref resultArray[0]);
                int elementCount = arrayLength * Unsafe.SizeOf<T>();
                Unsafe.CopyBlockUnaligned(baseAddress, source + offset, (uint)elementCount);
                offset += elementCount;
                return resultArray;
            }
            return default;
        }
    }


}