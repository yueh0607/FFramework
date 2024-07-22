using System;
using System.Runtime.InteropServices;

namespace FFramework
{
    public class Float32ReadWriteProvider : ReadWriteProvider<Single>
    {

        public override bool Read(ReadOnlySpan<byte> span, out Single value)
        {
            return MemoryMarshal.TryRead<Single>(span, out value);
        }

        public override bool Write(Span<byte> span, Single value)
        {
            return MemoryMarshal.TryWrite(span, ref value);
        }
    }
}
