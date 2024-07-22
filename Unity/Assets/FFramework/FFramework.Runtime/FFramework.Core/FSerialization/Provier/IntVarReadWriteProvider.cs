using System;

namespace FFramework
{
    public class IntVarReadWriteProvider<T> : ReadWriteProvider<T> where T : unmanaged
    {
        public override bool Read(ReadOnlySpan<byte> span, out T value)
        {
            
        }

        public override bool Write(Span<byte> span, T value)
        {
            
        }
    }
}
