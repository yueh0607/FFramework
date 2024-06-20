using System.Collections;
using System.Collections.Generic;

namespace FFramework
{
    public abstract class Branch : IBranch
    { 

        public abstract IEnumerator<Entity> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }



}
