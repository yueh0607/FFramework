using System;
using System.Runtime.InteropServices;

namespace FFramework
{
    public class UInt16ReadWriteProvider : ReadWriteProvider<UInt16>
    {
        public override bool Read(ReadOnlySpan<byte> span,out UInt16 value)
        {
            return MemoryMarshal.TryRead<UInt16>(span, out value);
        }

        public override bool Write(Span<byte> span, UInt16 value)
        {
            return MemoryMarshal.TryWrite(span, ref value);
        }
    }
}
