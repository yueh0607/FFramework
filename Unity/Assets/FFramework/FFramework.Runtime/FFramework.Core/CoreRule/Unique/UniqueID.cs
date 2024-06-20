using System;

namespace FFramework
{
    public class UniqueID 
    {
        private long m_NextID;
        
        public UniqueID(long startId)
        {
            m_NextID = startId;
        }

        public long GetNextID()
        {
            return m_NextID++;
        }

    }
}
