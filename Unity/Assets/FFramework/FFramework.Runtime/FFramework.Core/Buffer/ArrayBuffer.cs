using System;

namespace FFramework
{
    public struct ArrayBuffer<T>
    {
        public readonly T[] Buffer;
        public int InvalidLength;

        public readonly int TrueLength => Buffer.Length;

        public ArrayBuffer(T[] buffer,int invaliadLength)
        {
            this.Buffer = buffer;
            InvalidLength = invaliadLength;
        }

        public readonly ArrayBuffer<T> SetInvalidLength(int len)
        {
            return new ArrayBuffer<T>(Buffer,len);
        }
    }
}
