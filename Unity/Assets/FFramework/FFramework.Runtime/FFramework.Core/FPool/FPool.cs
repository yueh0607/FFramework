//using System;
//using System.Runtime.CompilerServices;


//namespace FFramework
//{
//    public static class FPool
//    {
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        internal static object InternalGet(Type type)
//            => Envirment.Current.GetModule<PoolModule>().InternalGet(type);
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static T Get<T>() where T : class
//            =>Envirment.Current.GetModule<PoolModule>().Get<T>();
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static T Get<T, K>() where T : class where K : IPoolable<T>, new()
//            => Envirment.Current.GetModule<PoolModule>().Get<T, K>();
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        internal static void InternalSet(Type type, object obj)
//            => Envirment.Current.GetModule<PoolModule>().InternalSet(type, obj);
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static void Set<T>(T obj) where T : class
//            => Envirment.Current.GetModule<PoolModule>().Set(obj);
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static void Set<T, K>(T obj) where T : class where K : IPoolable<T>, new()
//            => Envirment.Current.GetModule<PoolModule>().Set<T, K>(obj);
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static int Count<T>() where T : class
//            => Envirment.Current.GetModule<PoolModule>().Count<T>();
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static int Count<T, K>() where T : class where K : IPoolable<T>, new()
//            => Envirment.Current.GetModule<PoolModule>().Count<T, K>();

//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static void Release<T>() where T : class
//            => Envirment.Current.GetModule<PoolModule>().Release<T>();
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static void TryRelease<T>() where T : class
//            => Envirment.Current.GetModule<PoolModule>().TryRelease<T>();
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static void Create<T>(IPoolable<T> properties) where T : class
//            => Envirment.Current.GetModule<PoolModule>().Create(properties);
//    }
//}
