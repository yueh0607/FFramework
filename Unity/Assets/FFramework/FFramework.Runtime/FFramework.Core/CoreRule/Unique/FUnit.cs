namespace FFramework
{
    public abstract class FUnit
    {
        private static readonly UniqueID m_UniqueID = new UniqueID();

        private long m_ID;

        public long ID => m_ID;
        public FUnit()
        {
            m_ID = m_UniqueID.GetNextID();
        }

        internal void ResetID()
        {
            m_ID = m_UniqueID.GetNextID();
        }
    }
}
