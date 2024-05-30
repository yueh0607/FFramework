using FFramework.Internal;

namespace FFramework
{
    public interface ISendEvent : ISendEventBase,IGenericEventBase{}

    public interface ISendEvent<in T1> : ISendEventBase, IGenericEventBase { }

    public interface ISendEvent<in T1, in T2> : ISendEventBase, IGenericEventBase { }

    public interface ISendEvent<in T1, in T2, in T3> : ISendEventBase, IGenericEventBase { }

    public interface ISendEvent<in T1, in T2, in T3, in T4> : ISendEventBase, IGenericEventBase { }

    public interface ISendEvent<in T1, in T2, in T3, in T4, in T5> : ISendEventBase, IGenericEventBase { }

    public interface ISendEvent<in T1, in T2, in T3, in T4, in T5, in T6> : ISendEventBase, IGenericEventBase { }

    public interface ISendEvent<in T1, in T2, in T3, in T4, in T5, in T6, in T7> : ISendEventBase, IGenericEventBase { }

    public interface ISendEvent<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8> : ISendEventBase, IGenericEventBase { }


}
