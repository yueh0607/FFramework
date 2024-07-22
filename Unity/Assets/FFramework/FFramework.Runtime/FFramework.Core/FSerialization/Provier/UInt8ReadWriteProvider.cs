using System;
using System.Runtime.InteropServices;

namespace FFramework
{
    public class UInt8ReadWriteProvider : ReadWriteProvider<Byte>
    {
        public override bool Read(ReadOnlySpan<Byte> span,out Byte value)
        {
            return MemoryMarshal.TryRead<Byte>(span, out value);
        }

        public override bool Write(Span<Byte> span, Byte value)
        {
            return MemoryMarshal.TryWrite(span, ref value);
        }
    }
}
