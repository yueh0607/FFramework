using System;
using System.IO;

namespace FFramework
{
    public abstract class ReadWriteProvider<T>
    {

        public abstract bool Write(Span<byte> span, T value);

        public abstract bool Read(ReadOnlySpan<byte> span, out T value);

    }
}
