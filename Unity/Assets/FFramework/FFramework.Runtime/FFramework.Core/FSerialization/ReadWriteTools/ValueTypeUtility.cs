using System.Runtime.CompilerServices;

namespace FFramework
{
    internal class ValueTypeUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe T Read<T>(byte* source, ref int offset) where T : struct
        {
            var value = Unsafe.ReadUnaligned<T>(source + offset);
            offset += Unsafe.SizeOf<T>();
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void Write<T>(byte* destination, ref int offset, T value) where T : struct
        {
            Unsafe.WriteUnaligned(destination + offset, value);
            offset += Unsafe.SizeOf<T>();
        }
    }
}
