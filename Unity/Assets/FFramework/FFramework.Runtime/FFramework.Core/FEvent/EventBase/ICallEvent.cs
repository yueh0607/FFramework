using FFramework.Internal;

namespace FFramework
{
    public interface ICallEvent<out T1> : ICallEventBase, IGenericEventBase { }

    public interface ICallEvent<in T1,out T2> : ICallEventBase, IGenericEventBase { }

    public interface ICallEvent<in T1, in T2, out T3> : ICallEventBase, IGenericEventBase { }

    public interface ICallEvent<in T1, in T2,in T3,out T4> : ICallEventBase, IGenericEventBase { }

    public interface ICallEvent<in T1, out T2, in T3, in T4, out T5> : ICallEventBase, IGenericEventBase { }

    public interface ICallEvent<in T1, out T2, in T3, in T4, T5, out T6> : ICallEventBase, IGenericEventBase { }

    public interface ICallEvent<in T1, in T2, in T3, in T4, in T5, in T6, out T7> : ICallEventBase, IGenericEventBase { }

    public interface ICallEvent<in T1, in T2, in T3, in T4, in T5, in T6, in T7, out T8> : ICallEventBase, IGenericEventBase { }

    public interface ICallEvent<in T1, in T2, T3, in T4, in T5, in T6,in T7,in T8,out T9> : ICallEventBase, IGenericEventBase { }
}
