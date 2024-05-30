using System.Collections.Generic;

namespace FFramework
{

    /// <summary>
    /// 基本类型的内存拷贝器
    /// </summary>

    [SerializeProvider(typeof(int))]
    [SerializeProvider(typeof(long))]
    [SerializeProvider(typeof(float))]
    [SerializeProvider(typeof(double))]
    [SerializeProvider(typeof(bool))]
    [SerializeProvider(typeof(byte))]
    [SerializeProvider(typeof(char))]
    [SerializeProvider(typeof(short))]
    [SerializeProvider(typeof(uint))]
    [SerializeProvider(typeof(ulong))]
    [SerializeProvider(typeof(ushort))]
    [SerializeProvider(typeof(sbyte))]
    [SerializeProvider(typeof(byte))]
    internal class BaseStructProvider : ProviderInstance<BaseStructProvider>
    {
        public override unsafe void Deserialize<K>(void* source, void* address)
        {
            
        }

        public override unsafe void Serialize<K>(void* destination, void* address)
        {
            
        }
    }


    /// <summary>
    /// 基本类型数组的内存拷贝器
    /// </summary>

    [SerializeProvider(typeof(int[]))]
    [SerializeProvider(typeof(long[]))]
    [SerializeProvider(typeof(float[]))]
    [SerializeProvider(typeof(double[]))]
    [SerializeProvider(typeof(bool[]))]
    [SerializeProvider(typeof(byte[]))]
    [SerializeProvider(typeof(char[]))]
    [SerializeProvider(typeof(short[]))]
    [SerializeProvider(typeof(uint[]))]
    [SerializeProvider(typeof(ulong[]))]
    [SerializeProvider(typeof(ushort[]))]
    [SerializeProvider(typeof(sbyte[]))]
    [SerializeProvider(typeof(byte[]))]
    internal class BaseStructArrayProvider : ProviderInstance<BaseStructProvider>
    {
        public override unsafe void Deserialize<K>(void* source, void* address)
        {

        }

        public override unsafe void Serialize<K>(void* destination, void* address)
        {

        }
    }


}
