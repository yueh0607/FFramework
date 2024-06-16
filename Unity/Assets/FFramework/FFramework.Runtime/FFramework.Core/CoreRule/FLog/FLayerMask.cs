using System;

namespace FFramework
{
    /// <summary>
    /// 支持最多0-63层的开闭
    /// </summary>
    public struct FLayerMask

    {
        private long m_LayerMask;

        public FLayerMask(long layerMask)
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

        // Check if a specific layer is open (bit is set)
        public bool IsLayerOpen(int layer)
        {
            return (m_LayerMask & (1L << layer)) != 0;
        }

        // Check if a specific layer is closed (bit is clear)
        public bool IsLayerClosed(int layer)
        {
            return (m_LayerMask & (1L << layer)) == 0;
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

   
        // Check equality between two FLayerMask objects
        public static bool operator ==(FLayerMask left, FLayerMask right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null)) return false;
            return left.m_LayerMask == right.m_LayerMask;
        }

        // Check inequality between two FLayerMask objects
        public static bool operator !=(FLayerMask left, FLayerMask right)
        {
            return !(left == right);
        }

        // Override Equals method
        public override bool Equals(object obj)
        {
            if (obj is FLayerMask mask)
            {
                return m_LayerMask == mask.m_LayerMask;
            }
            return false;
        }

        // Override GetHashCode method
        public override int GetHashCode()
        {
            return m_LayerMask.GetHashCode();
        }

        public const long AllOpenedValue =~0L;
        public const long AllClosedValue = 0L;

        public void CloseAll()
        {
            m_LayerMask = AllClosedValue;
        }

        public void OpenAll()
        {
            m_LayerMask = AllOpenedValue;
        }
    }
}
