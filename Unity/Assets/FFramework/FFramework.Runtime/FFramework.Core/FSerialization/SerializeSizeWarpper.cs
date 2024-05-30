



namespace FFramework.Serialization
{
    using System.Runtime.CompilerServices;
    using SIZE =

#if SERIALIZE_SIZE_32
    System.Int32;
#else
    System.UInt16
#endif
;


    internal class SerializeSizeWarpper
    {
        private SerializeSizeWarpper() { }

        internal const sbyte LENGTH = sizeof(SIZE);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static SIZE ConvertInt(int value)
            => (SIZE)value;


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void WriteUnaligned(void* destination, SIZE value)
        {
            Unsafe.WriteUnaligned(destination, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe SIZE ReadUnaligned(void* source)
        {
            return Unsafe.ReadUnaligned<SIZE>(source);
        }

    }
}
