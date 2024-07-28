using System;
using System.Buffers;
using System.IO;

namespace FFramework
{
    public abstract class ReadWriteProvider<T>
    {

        public abstract void Write(DynamicSequence sequence, T value);

        public abstract T Read(DynamicSequence sequence);

    }
}
