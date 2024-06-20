using System;

namespace FFramework
{
    public abstract class FUnit
    {
        //将0和以前的ID作为保留ID
        private static readonly Lazy<UniqueID> m_UniqueID = new Lazy<UniqueID>(()=>new UniqueID(1));

        private long m_ID;

        public long ID => m_ID;
        public FUnit()
        {
            m_ID = m_UniqueID.Value.GetNextID();
        }

        internal void ResetID()
        {
            m_ID = m_UniqueID.Value.GetNextID();
        }
    }
}
