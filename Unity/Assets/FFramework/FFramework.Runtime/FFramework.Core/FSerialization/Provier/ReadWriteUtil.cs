using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FFramework.Serialization.Binary
{
    public static class ReadWriteUtil
    {
        public static T Read<T>(ref DynamicSequence sequence)
        {
            int size = Unsafe.SizeOf<T>();
            sequence.ReadMove(size, out ReadOnlySpan<byte> pos);
            ref byte span = ref MemoryMarshal.GetReference(pos);
            return Unsafe.ReadUnaligned<T>(ref span);
        }

        public static void Write<T>(ref DynamicSequence sequence,T value)
        {
            int size = Unsafe.SizeOf<T>();
            sequence.WriteMove(size, out Span<byte> pos);
            ref byte span = ref MemoryMarshal.GetReference(pos);
            Unsafe.WriteUnaligned(ref span,value);
        }
    }
}
