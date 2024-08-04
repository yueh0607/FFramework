using System;
using System.Buffers;
namespace FFramework
{
    public sealed class ArrayWrapper<T> : IPoolRecyclable
    {
        public T[] Buffer { get; private set; } = Array.Empty<T>();

        public int Length => Buffer.Length;

        public void Preserve(int size)
        {
            Buffer = ArrayPool<T>.Shared.Rent(size);
        }

        public static explicit operator T[](ArrayWrapper<T> wrapper)
        {
            return wrapper.Buffer;
        }

        public Span<T> AsSpan(int start,int length)
        {
            return Buffer.AsSpan(start, length);
        }

        public Span<T> AsSpan() => AsSpan(0, Buffer.Length);

        public void Clear()
        {
            Array.Clear(Buffer, 0, Buffer.Length);
        }

    
        public T this[int index]
        {
            get
            {
                return Buffer[index];
            }
            set
            {
                Buffer[index] = value;
            }
        }

        public class Poolable : IPoolable<ArrayWrapper<T>>
        {
            public int Capacity => 1000;

            public ArrayWrapper<T> OnCreate()
            {
                return new ArrayWrapper<T>();
            }

            public void OnDestroy(ArrayWrapper<T> obj)
            {

            }

            public void OnGet(ArrayWrapper<T> obj)
            {

            }

            public void OnSet(ArrayWrapper<T> obj)
            {
                ArrayPool<T>.Shared.Return(obj.Buffer);
                obj.Buffer = null;
            }
        }
    }
}
