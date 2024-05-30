using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace FFramework.Serialization
{
    internal class StringUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void Write(byte* destination, ref int offset, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var charSpan = value.AsSpan();
                var byteSpan = new Span<byte>(
                    destination + offset + SerializeSizeWarpper.LENGTH, value.Length * 3);
                var count = Encoding.UTF8.GetBytes(charSpan, byteSpan);
                SerializeSizeWarpper.WriteUnaligned(destination + offset,
                    SerializeSizeWarpper.ConvertInt(count));
                offset += SerializeSizeWarpper.LENGTH + count;
            }
            else
            {
                SerializeSizeWarpper.WriteUnaligned(destination + offset, 0);
                offset += SerializeSizeWarpper.LENGTH;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe string Read(byte* source, ref int offset)
        {
            var count = SerializeSizeWarpper.ReadUnaligned(source + offset);
            offset += SerializeSizeWarpper.LENGTH;
            if (count > 0)
            {
                var value = Encoding.UTF8.GetString(
                    new ReadOnlySpan<byte>(source + offset, count));
                offset += count;
                return value;
            }
            return string.Empty;
        }
    }
}
