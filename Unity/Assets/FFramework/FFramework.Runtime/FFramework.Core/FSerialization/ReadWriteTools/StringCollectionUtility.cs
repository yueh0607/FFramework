using FFramework.Serialization;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace FFramework
{
    public class StringCollectionUtility
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void WriteCollection(byte* destination, ref int offset, ICollection<string> value)
        {
            if (value != null)
            {
                int len = value.Count;
                SerializeSizeWarpper.WriteUnaligned(destination + offset, 
                    SerializeSizeWarpper.ConvertInt(len));
                offset +=SerializeSizeWarpper. LENGTH;
                var enumerator = value.GetEnumerator();
                while (enumerator.MoveNext())
                   StringUtility.Write(destination, ref offset, enumerator.Current);
            }
            else
            {
                SerializeSizeWarpper.WriteUnaligned(destination + offset, 0);
                offset +=SerializeSizeWarpper.LENGTH;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe T ReadCollection<T>(byte* ptr, ref int offset) where T : ICollection<string>, new()
        {
            var newValue = StringArrayUtility.ReadArray(ptr, ref offset);
            if (newValue == null)
                return default;
            var value = new T();
            for (int i = 0; i < newValue.Length; i++)
                value.Add(newValue[i]);
            return value;
        }

    }
}
