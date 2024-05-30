namespace FFramework.Serialization
{
    public unsafe interface IDeserializableProvider 
    {
        /// <summary>
        /// 将"source"处的二进制数据恢复到"address"处
        /// </summary>
        /// <param name="source"></param>
        /// <param name="address"></param>
        void Deserialize<T>(void* source,void* address);
    }
}
