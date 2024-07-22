using System;
using System.Runtime.InteropServices;

namespace FFramework
{
    public class Int32ReadWriteProvider : ReadWriteProvider<Int32>
    {

        public override bool Read(ReadOnlySpan<byte> span, out Int32 value)
        {
            return MemoryMarshal.TryRead<Int32>(span, out value);
        }

        public override bool Write(Span<byte> span, Int32 value)
        {
            

            return MemoryMarshal.TryWrite(span, ref value);
        }
    }
}
