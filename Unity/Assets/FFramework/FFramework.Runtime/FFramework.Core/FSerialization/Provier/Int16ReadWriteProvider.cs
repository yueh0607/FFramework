using System;
using System.Runtime.InteropServices;

namespace FFramework
{
    public class Int16ReadWriteProvider : ReadWriteProvider<Int16>
    {
        public override bool Read(ReadOnlySpan<byte> span,out Int16 value)
        {
            return MemoryMarshal.TryRead<Int16>(span, out value);
        }
        public override bool Write(Span<byte> span, Int16 value)
        {
            return MemoryMarshal.TryWrite(span, ref value);
        }
    }
}
