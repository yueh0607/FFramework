using System.Buffers;

namespace FFramework
{
    public interface IMemoryAllocator 
    {
        IMemoryOwner<byte> Allocate(int minLength);

        void Release(IMemoryOwner<byte> memoryOwner);
    }
}
