using System;

namespace FFramework
{
    public class UniqueID 
    {
        private long m_NextID = long.MaxValue;
        
        public UniqueID()
        {

        }

        public long GetNextID()
        {
            return m_NextID--;
        }

    }
}
