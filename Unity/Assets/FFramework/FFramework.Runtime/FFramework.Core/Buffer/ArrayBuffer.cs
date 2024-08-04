using System;

namespace FFramework
{
    public struct ArrayBuffer<T>
    {
        public readonly ArrayWrapper<T> Buffer;
        public int InvalidLength;

        public readonly int TrueLength => Buffer.Length;

        public ArrayBuffer(ArrayWrapper<T> buffer,int invaliadLength)
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
