using System;

namespace FFramework
{
    /// <summary>
    /// 支持最多0-63层的开闭
    /// </summary>
#pragma warning disable CS0660 // 类型定义运算符 == 或运算符 !=，但不重写 Object.Equals(object o)
#pragma warning disable CS0661 // 类型定义运算符 == 或运算符 !=，但不重写 Object.GetHashCode()
    public struct FLayerMask
#pragma warning restore CS0661 // 类型定义运算符 == 或运算符 !=，但不重写 Object.GetHashCode()
#pragma warning restore CS0660 // 类型定义运算符 == 或运算符 !=，但不重写 Object.Equals(object o)
    {
        private long m_LayerMask;

        private FLayerMask(long layerMask)
        {
            m_LayerMask = layerMask;
        }

        // 打开N层
        public static FLayerMask operator >>(FLayerMask mask, int layer)
        {
            return new FLayerMask(mask.m_LayerMask | (1L << layer));
        }

        // 关闭N层
        public static FLayerMask operator <<(FLayerMask mask, int layer)
        {
            return new FLayerMask(mask.m_LayerMask & ~(1L << layer));
        }

        // 是否打开了N层
        public static bool operator ==(FLayerMask mask, int layer)
        {
            return (mask.m_LayerMask & (1L << layer)) != 0;
        }

        // 是否关闭了N层
        public static bool operator !=(FLayerMask mask, int layer)
        {
            return (mask.m_LayerMask & (1L << layer)) == 0;
        }

        public static bool operator ==(FLayerMask left, FLayerMask right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(FLayerMask left, FLayerMask right)
        {
            return !(left == right);
        }
    }
}
