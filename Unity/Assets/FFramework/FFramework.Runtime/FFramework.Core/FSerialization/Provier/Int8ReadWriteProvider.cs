using System;
using System.Runtime.InteropServices;

namespace FFramework
{
    public class Int8ReadWriteProvider : ReadWriteProvider<SByte>
    {
        public override bool Read(ReadOnlySpan<byte> span,out SByte value)
        {
            return MemoryMarshal.TryRead(span, out value);
        }

        public override bool Write(Span<byte> span, SByte value)
        {
            return MemoryMarshal.TryWrite<SByte>(span,ref value);
        }
    }
}
