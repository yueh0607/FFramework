using System;
using System.Runtime.InteropServices;

namespace FFramework
{
    public class UInt32ReadWriteProvider : ReadWriteProvider<UInt32>
    {

        public override bool Read(ReadOnlySpan<byte> span, out UInt32 value)
        {
            return MemoryMarshal.TryRead<UInt32>(span, out value);
        }

        public override bool Write(Span<byte> span, UInt32 value)
        {
            return MemoryMarshal.TryWrite(span, ref value);
        }
    }
}
