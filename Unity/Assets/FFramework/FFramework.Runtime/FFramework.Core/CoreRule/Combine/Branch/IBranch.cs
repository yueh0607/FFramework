using System.Collections;
using System.Collections.Generic;

namespace FFramework
{
    public interface IBranch : IEnumerable, IEnumerable<Entity>
    {
        void AddChild(IEntity entity);

        void RemoveChild(IEntity entity);
    }
}
