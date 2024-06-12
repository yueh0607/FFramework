using System.Collections.Generic;
public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ AOT assemblies
	public static readonly IReadOnlyList<string> PatchedAOTAssemblyList = new List<string>
	{
		"FFramework.AOT.dll",
		"System.Core.dll",
		"System.Runtime.CompilerServices.Unsafe.dll",
		"System.dll",
		"UnityEngine.CoreModule.dll",
		"mscorlib.dll",
	};
	// }}

	// {{ constraint implement type
	// }} 

	// {{ AOT generic types
	// System.Action<System.TimeSpan>
	// System.Action<int>
	// System.Action<object,object>
	// System.Action<object>
	// System.ArraySegment.Enumerator<byte>
	// System.ArraySegment<byte>
	// System.Buffers.ArrayMemoryPool.ArrayMemoryPoolBuffer<byte>
	// System.Buffers.ArrayMemoryPool<byte>
	// System.Buffers.ArrayPool<byte>
	// System.Buffers.MemoryManager<byte>
	// System.Buffers.MemoryPool<byte>
	// System.Buffers.TlsOverPerCoreLockedStacksArrayPool.LockedStack<byte>
	// System.Buffers.TlsOverPerCoreLockedStacksArrayPool.PerCoreLockedStacks<byte>
	// System.Buffers.TlsOverPerCoreLockedStacksArrayPool<byte>
	// System.ByReference<byte>
	// System.Collections.Concurrent.ConcurrentDictionary.<GetEnumerator>d__35<int,object>
	// System.Collections.Concurrent.ConcurrentDictionary.DictionaryEnumerator<int,object>
	// System.Collections.Concurrent.ConcurrentDictionary.Node<int,object>
	// System.Collections.Concurrent.ConcurrentDictionary.Tables<int,object>
	// System.Collections.Concurrent.ConcurrentDictionary<int,object>
	// System.Collections.Concurrent.ConcurrentQueue.<Enumerate>d__28<object>
	// System.Collections.Concurrent.ConcurrentQueue.Segment<object>
	// System.Collections.Concurrent.ConcurrentQueue<object>
	// System.Collections.Generic.ArraySortHelper<int>
	// System.Collections.Generic.ArraySortHelper<object>
	// System.Collections.Generic.Comparer<int>
	// System.Collections.Generic.Comparer<object>
	// System.Collections.Generic.Dictionary.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection<object,object>
	// System.Collections.Generic.Dictionary<object,object>
	// System.Collections.Generic.EqualityComparer<int>
	// System.Collections.Generic.EqualityComparer<object>
	// System.Collections.Generic.HashSet.Enumerator<object>
	// System.Collections.Generic.HashSet<object>
	// System.Collections.Generic.HashSetEqualityComparer<object>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ICollection<int>
	// System.Collections.Generic.ICollection<object>
	// System.Collections.Generic.IComparer<int>
	// System.Collections.Generic.IComparer<object>
	// System.Collections.Generic.IDictionary<int,object>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.UIntPtr,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerable<int>
	// System.Collections.Generic.IEnumerable<object>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.UIntPtr,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerator<int>
	// System.Collections.Generic.IEnumerator<object>
	// System.Collections.Generic.IEqualityComparer<int>
	// System.Collections.Generic.IEqualityComparer<object>
	// System.Collections.Generic.IList<int>
	// System.Collections.Generic.IList<object>
	// System.Collections.Generic.KeyValuePair<System.UIntPtr,object>
	// System.Collections.Generic.KeyValuePair<int,object>
	// System.Collections.Generic.KeyValuePair<object,object>
	// System.Collections.Generic.LinkedList.Enumerator<object>
	// System.Collections.Generic.LinkedList<object>
	// System.Collections.Generic.LinkedListNode<object>
	// System.Collections.Generic.List.Enumerator<int>
	// System.Collections.Generic.List.Enumerator<object>
	// System.Collections.Generic.List<int>
	// System.Collections.Generic.List<object>
	// System.Collections.Generic.ObjectComparer<int>
	// System.Collections.Generic.ObjectComparer<object>
	// System.Collections.Generic.ObjectEqualityComparer<int>
	// System.Collections.Generic.ObjectEqualityComparer<object>
	// System.Collections.Generic.Queue.Enumerator<System.ValueTuple<int,object>>
	// System.Collections.Generic.Queue.Enumerator<object>
	// System.Collections.Generic.Queue<System.ValueTuple<int,object>>
	// System.Collections.Generic.Queue<object>
	// System.Collections.ObjectModel.ReadOnlyCollection<int>
	// System.Collections.ObjectModel.ReadOnlyCollection<object>
	// System.Comparison<int>
	// System.Comparison<object>
	// System.Func<byte>
	// System.Func<int,object,object>
	// System.Func<int,object>
	// System.Func<object,byte>
	// System.Func<object,object>
	// System.Func<object>
	// System.IProgress<float>
	// System.Memory<byte>
	// System.Predicate<int>
	// System.Predicate<object>
	// System.ReadOnlyMemory<byte>
	// System.ReadOnlySpan.Enumerator<byte>
	// System.ReadOnlySpan<byte>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable.ConfiguredTaskAwaiter<object>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable<object>
	// System.Runtime.CompilerServices.TaskAwaiter<object>
	// System.Span.Enumerator<byte>
	// System.Span<byte>
	// System.Threading.Tasks.ContinuationTaskFromResultTask<object>
	// System.Threading.Tasks.Task<object>
	// System.Threading.Tasks.TaskFactory<object>
	// System.ValueTuple<int,object>
	// }}

	public void RefMethods()
	{
		// object System.Activator.CreateInstance<object>()
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,FFramework.ThreadingPromise.<RunTask>d__19>(System.Runtime.CompilerServices.TaskAwaiter&,FFramework.ThreadingPromise.<RunTask>d__19&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,FFramework.ThreadingPromise.<RunTask>d__19<object>>(System.Runtime.CompilerServices.TaskAwaiter<object>&,FFramework.ThreadingPromise.<RunTask>d__19<object>&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<FFramework.ThreadingPromise.<RunTask>d__19<object>>(FFramework.ThreadingPromise.<RunTask>d__19<object>&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<FFramework.ThreadingPromise.<RunTask>d__19>(FFramework.ThreadingPromise.<RunTask>d__19&)
		// System.Void* System.Runtime.CompilerServices.Unsafe.AsPointer<object>(object&)
		// object System.Runtime.CompilerServices.Unsafe.ReadUnaligned<object>(System.Void*)
		// ushort System.Runtime.CompilerServices.Unsafe.ReadUnaligned<ushort>(System.Void*)
		// int System.Runtime.CompilerServices.Unsafe.SizeOf<object>()
		// System.Void System.Runtime.CompilerServices.Unsafe.WriteUnaligned<object>(System.Void*,object)
		// System.Void System.Runtime.CompilerServices.Unsafe.WriteUnaligned<ushort>(System.Void*,ushort)
		// object UnityEngine.GameObject.AddComponent<object>()
		// YooAsset.AssetHandle YooAsset.ResourcePackage.LoadAssetAsync<object>(string,uint)
		// YooAsset.SubAssetsHandle YooAsset.ResourcePackage.LoadSubAssetsAsync<object>(string,uint)
		// YooAsset.AssetHandle YooAsset.YooAssets.LoadAssetAsync<object>(string,uint)
		// YooAsset.SubAssetsHandle YooAsset.YooAssets.LoadSubAssetsAsync<object>(string,uint)
	}
}