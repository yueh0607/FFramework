namespace FFramework.Serialization
{
    public unsafe interface ISerializableProvider
    {
        /// <summary>
        /// 提供将 "address"处 的数据按固定格式序列化到 "destination" 处的能力
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="address"></param>
        void Serialize<T>(void* destination,void* address);

    }
}
