using System;
using System.Runtime.InteropServices;

namespace FFramework
{
    public class UInt64ReadWriteProvider : ReadWriteProvider<UInt64>
    {

        public override bool Read(ReadOnlySpan<byte> span, out UInt64 value)
        {
            return MemoryMarshal.TryRead<UInt64>(span, out value);
        }

        public override bool Write(Span<byte> span, UInt64 value)
        {
            return MemoryMarshal.TryWrite(span, ref value);
        }
    }
}
