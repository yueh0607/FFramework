using System.Buffers;

namespace FFramework
{
    public class MemoryPoolAllocator : IMemoryAllocator
    {
        public IMemoryOwner<byte> Allocate(int minBufferSize)
        {
            return MemoryPool<byte>.Shared.Rent(minBufferSize);
        }

        public void Release(IMemoryOwner<byte> memoryOwner)
        {
            memoryOwner.Dispose();
        }
    }
}
