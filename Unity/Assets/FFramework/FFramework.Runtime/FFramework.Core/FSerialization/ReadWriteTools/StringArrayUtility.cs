using System.Runtime.CompilerServices;

namespace FFramework.Serialization
{
    internal class StringArrayUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void WriteArray(byte* destination, ref int offset, string[] value)
        {
            if (value != null)
            {
                int arrayLength = value.Length;
                SerializeSizeWarpper.WriteUnaligned(destination + offset,
                    SerializeSizeWarpper.ConvertInt(arrayLength));
                offset += SerializeSizeWarpper.LENGTH;
                for (int i = 0; i < arrayLength; i++)
                    StringUtility.Write(destination, ref offset, value[i]);
            }
            else
            {
                SerializeSizeWarpper.WriteUnaligned(destination + offset, 0);
                offset += SerializeSizeWarpper.LENGTH;
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe string[] ReadArray(byte* source, ref int offset)
        {
            var arrayLength = SerializeSizeWarpper.ReadUnaligned(source + offset);
            offset += SerializeSizeWarpper.LENGTH;
            if (arrayLength > 0)
            {
                var value = new string[arrayLength];
                for (int i = 0; i < arrayLength; i++)
                    value[i] = StringUtility.Read(source, ref offset);
                return value;
            }
            return default;
        }

    }
}
