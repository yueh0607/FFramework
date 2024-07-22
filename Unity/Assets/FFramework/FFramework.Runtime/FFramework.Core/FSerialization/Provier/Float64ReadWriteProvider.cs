using System;
using System.Runtime.InteropServices;

namespace FFramework
{
    public class Float64ReadWriteProvider : ReadWriteProvider<Double>
    {

        public override bool Read(ReadOnlySpan<byte> span, out Double value)
        {
            return MemoryMarshal.TryRead<Double>(span, out value);
        }

        public override bool Write(Span<byte> span, Double value)
        {
            return MemoryMarshal.TryWrite(span, ref value);
        }
    }
}
