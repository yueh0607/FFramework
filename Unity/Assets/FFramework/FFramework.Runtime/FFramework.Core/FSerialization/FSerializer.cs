//using System;
//using System.Buffers;

//namespace FFramework
//{
//    public sealed class FSerializer
//    {


//        private static MemoryPoolAllocator m_PoolAllocator = new MemoryPoolAllocator();

//        private FSerializer() { }

//        public static ValueTuple<byte[],int> Serialize<T>(T value)
//        {

//        }

//        public static ValueTuple<byte[], int> SerializeByRef<T>(ref T value) where T : struct
//        {

//        }



//        public static ValueTuple<IMemoryOwner<byte>,int> Serialize<T>(T value, IMemoryAllocator allocator = null)
//        {
//            IMemoryAllocator m_CurrentAllocator = allocator ?? m_PoolAllocator;


//        }

//        public static ValueTuple<IMemoryOwner<byte>, int> SerializeByRef<T>(ref T value, IMemoryAllocator allocator = null) where T : struct
//        {
//            IMemoryAllocator m_CurrentAllocator = allocator ?? m_PoolAllocator;


//        }


//        //计算序列化后的大小
//        public static int CalculateSize<T>(T value)
//        {

//        }

//    }
//}
