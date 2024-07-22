using System;
using System.Runtime.InteropServices;

namespace FFramework
{
    public class Int64ReadWriteProvider : ReadWriteProvider<Int64>
    {

        public override bool Read(ReadOnlySpan<byte> span, out Int64 value)
        {
            return MemoryMarshal.TryRead<Int64>(span, out value);
        }

        public override bool Write(Span<byte> span, Int64 value)
        {
            

            return MemoryMarshal.TryWrite(span, ref value);
        }
    }
}
