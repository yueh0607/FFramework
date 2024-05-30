namespace FFramework
{

    public struct UnitRef<T> where T : FUnit
    {
        T m_Unit;
        long m_InitID;

        public T Value
        {
            get
            {
                if(m_Unit.ID!= m_InitID)
                {
                    return null;
                }
                return m_Unit;
            }
        }

        public UnitRef(T unit)
        {
            this.m_Unit = unit;
            this.m_InitID = unit.ID;
        }

        public readonly UnitRef<K> To<K>() where K : T
        {
            return new UnitRef<K>(m_Unit as K);
        }

        public static implicit operator T(UnitRef<T> unitRef)
        {
            return unitRef.Value;
        }

        public static implicit operator UnitRef<T>(T unit)
        {
            return new UnitRef<T>(unit);
        }
    }
}
