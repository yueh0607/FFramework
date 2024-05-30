﻿using MemoryPack.Formatters;
using MemoryPack.Internal;
using System.Buffers;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Immutable Collections formatters

namespace MemoryPack
{
    public static partial class MemoryPackFormatterProvider
    {
        static readonly Dictionary<Type, Type> ImmutableCollectionFormatters = new Dictionary<Type, Type>()
        {
            { typeof(ImmutableArray<>), typeof(ImmutableArrayFormatter<>) },
            { typeof(ImmutableList<>), typeof(ImmutableListFormatter<>) },
            { typeof(ImmutableQueue<>), typeof(ImmutableQueueFormatter<>) },
            { typeof(ImmutableStack<>), typeof(ImmutableStackFormatter<>) },
            { typeof(ImmutableDictionary<,>), typeof(ImmutableDictionaryFormatter<,>) },
            { typeof(ImmutableSortedDictionary<,>), typeof(ImmutableSortedDictionaryFormatter<,>) },
            { typeof(ImmutableSortedSet<>), typeof(ImmutableSortedSetFormatter<>) },
            { typeof(ImmutableHashSet<>), typeof(ImmutableHashSetFormatter<>) },
            { typeof(IImmutableList<>), typeof(InterfaceImmutableListFormatter<>) },
            { typeof(IImmutableQueue<>), typeof(InterfaceImmutableQueueFormatter<>) },
            { typeof(IImmutableStack<>), typeof(InterfaceImmutableStackFormatter<>) },
            { typeof(IImmutableDictionary<,>), typeof(InterfaceImmutableDictionaryFormatter<,>) },
            { typeof(IImmutableSet<>), typeof(InterfaceImmutableSetFormatter<>) },
        };
    }
}

namespace MemoryPack.Formatters
{
    [Preserve]
    public sealed class ImmutableArrayFormatter<T> : MemoryPackFormatter<ImmutableArray<T?>>
    {
        [Preserve]
        public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref ImmutableArray<T?> value)
        {
            if (value.IsDefault)
            {
                writer.WriteNullCollectionHeader();
            }
            else
            {
                writer.WriteSpan(value.AsSpan());
            }
        }

        [Preserve]
        public override void Deserialize(ref MemoryPackReader reader, scoped ref ImmutableArray<T?> value)
        {
            var array = reader.ReadArray<T?>();
            if (array == null)
            {
                value = default;
                return;
            }

            if (array.Length == 0)
            {
                value = ImmutableArray<T?>.Empty;
                return;
            }

#if NET8_0_OR_GREATER
            value = ImmutableCollectionsMarshal.AsImmutableArray(array);
#else
            // create Empty and replace inner T[] field(avoid defensive copy of Create)
            value = ImmutableArray.Create<T?>();
            ref var view = ref Unsafe.As<ImmutableArray<T?>, ImmutableArrayView<T?>>(ref value);
            view.array = array;
#endif
        }
    }

    [Preserve]
    internal struct ImmutableArrayView<T>
    {
        public T[]? array;
    }

    [Preserve]
    public sealed class ImmutableListFormatter<T> : MemoryPackFormatter<ImmutableList<T?>>
    {
        [Preserve]
        public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref ImmutableList<T?>? value)
        {
            if (value == null)
            {
                writer.WriteNullCollectionHeader();
                return;
            }

            var formatter = writer.GetFormatter<T?>();
            writer.WriteCollectionHeader(value.Count);
            foreach (var item in value)
            {
                var v = item;
                formatter.Serialize(ref writer, ref v);
            }
        }

        [Preserve]
        public override void Deserialize(ref MemoryPackReader reader, scoped ref ImmutableList<T?>? value)
        {
            if (!reader.TryReadCollectionHeader(out var length))
            {
                value = null;
                return;
            }

            if (length == 0)
            {
                value = ImmutableList<T?>.Empty;
                return;
            }

            if (length == 1)
            {
                var item = reader.ReadValue<T>();
                value = ImmutableList.Create(item);
                return;
            }

            var formatter = reader.GetFormatter<T?>();

            var builder = ImmutableList.CreateBuilder<T?>();
            for (int i = 0; i < length; i++)
            {
                T? item = default;
                formatter.Deserialize(ref reader, ref item);
                builder.Add(item);
            }

            value = builder.ToImmutable();
        }
    }

    [Preserve]
    public sealed class ImmutableQueueFormatter<T> : MemoryPackFormatter<ImmutableQueue<T?>>
    {
        [Preserve]
        public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref ImmutableQueue<T?>? value)
        {
            if (value == null)
            {
                writer.WriteNullCollectionHeader();
                return;
            }

            // ImmutableQueue<T> has no Count, so use similar serialization of IEnumerable<T>

            var tempBuffer = ReusableLinkedArrayBufferWriterPool.Rent();
            try
            {
                var tempWriter = new MemoryPackWriter<ReusableLinkedArrayBufferWriter>(ref tempBuffer, writer.OptionalState);

                var count = 0;
                var formatter = writer.GetFormatter<T?>();
                foreach (var item in value)
                {
                    count++;
                    var v = item;
                    formatter.Serialize(ref tempWriter, ref v);
                }

                tempWriter.Flush();

                // write to parameter writer.
                writer.WriteCollectionHeader(count);
                tempBuffer.WriteToAndReset(ref writer);
            }
            finally
            {
                ReusableLinkedArrayBufferWriterPool.Return(tempBuffer);
            }
        }

        [Preserve]
        public override void Deserialize(ref MemoryPackReader reader, scoped ref ImmutableQueue<T?>? value)
        {
            if (!reader.TryReadCollectionHeader(out var length))
            {
                value = null;
                return;
            }

            if (length == 0)
            {
                value = ImmutableQueue<T?>.Empty;
                return;
            }

            if (length == 1)
            {
                var item = reader.ReadValue<T>();
                value = ImmutableQueue.Create(item);
                return;
            }

            // ImmutableQueue<T> has no builder

            var rentArray = ArrayPool<T?>.Shared.Rent(length);
            try
            {
                var formatter = reader.GetFormatter<T?>();
                for (int i = 0; i < length; i++)
                {
                    formatter.Deserialize(ref reader, ref rentArray[i]);
                }

                if (rentArray.Length == length)
                {
                    // we can use T[] ctor
                    value = ImmutableQueue.Create(rentArray);
                    return;
                }
                else
                {
                    // IEnumerable<T> method
                    value = ImmutableQueue.CreateRange((new ArraySegment<T?>(rentArray, 0, length)).AsEnumerable());
                }
            }
            finally
            {
                ArrayPool<T?>.Shared.Return(rentArray, clearArray: RuntimeHelpers.IsReferenceOrContainsReferences<T>());
            }
        }
    }

    [Preserve]
    public sealed class ImmutableStackFormatter<T> : MemoryPackFormatter<ImmutableStack<T?>>
    {
        [Preserve]
        public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref ImmutableStack<T?>? value)
        {
            if (value == null)
            {
                writer.WriteNullCollectionHeader();
                return;
            }

            // ImmutableStack<T> has no Count, so use similar serialization of IEnumerable<T>

            var tempBuffer = ReusableLinkedArrayBufferWriterPool.Rent();
            try
            {
                var tempWriter = new MemoryPackWriter<ReusableLinkedArrayBufferWriter>(ref tempBuffer, writer.OptionalState);

                var count = 0;
                var formatter = writer.GetFormatter<T?>();

                foreach (var item in value.AsEnumerable().Reverse()) // serialize reverse order
                {
                    count++;
                    var v = item;
                    formatter.Serialize(ref tempWriter, ref v);
                }

                tempWriter.Flush();

                // write to parameter writer.
                writer.WriteCollectionHeader(count);
                tempBuffer.WriteToAndReset(ref writer);
            }
            finally
            {
                ReusableLinkedArrayBufferWriterPool.Return(tempBuffer);
            }
        }

        [Preserve]
        public override void Deserialize(ref MemoryPackReader reader, scoped ref ImmutableStack<T?>? value)
        {
            if (!reader.TryReadCollectionHeader(out var length))
            {
                value = null;
                return;
            }

            if (length == 0)
            {
                value = ImmutableStack<T?>.Empty;
                return;
            }

            if (length == 1)
            {
                var item = reader.ReadValue<T>();
                value = ImmutableStack.Create(item);
                return;
            }

            // ImmutableStack<T> has no builder

            var rentArray = ArrayPool<T?>.Shared.Rent(length);
            try
            {
                var formatter = reader.GetFormatter<T?>();
                for (int i = 0; i < length; i++)
                {
                    formatter.Deserialize(ref reader, ref rentArray[i]);
                }

                if (rentArray.Length == length)
                {
                    // we can use T[] ctor
                    value = ImmutableStack.Create(rentArray);
                    return;
                }
                else
                {
                    // IEnumerable<T> method
                    value = ImmutableStack.CreateRange((new ArraySegment<T?>(rentArray, 0, length)).AsEnumerable());
                }
            }
            finally
            {
                ArrayPool<T?>.Shared.Return(rentArray, clearArray: RuntimeHelpers.IsReferenceOrContainsReferences<T>());
            }
        }
    }

    [Preserve]
    public sealed class ImmutableDictionaryFormatter<TKey, TValue> : MemoryPackFormatter<ImmutableDictionary<TKey, TValue?>?>
        where TKey : notnull
    {
        readonly IEqualityComparer<TKey>? keyEqualityComparer;
        readonly IEqualityComparer<TValue?>? valueEqualityComparer;

        public ImmutableDictionaryFormatter()
            : this(null, null)
        {

        }

        public ImmutableDictionaryFormatter(IEqualityComparer<TKey>? keyEqualityComparer, IEqualityComparer<TValue?>? valueEqualityComparer)
        {
            this.keyEqualityComparer = keyEqualityComparer;
            this.valueEqualityComparer = valueEqualityComparer;
        }

        [Preserve]
        public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref ImmutableDictionary<TKey, TValue?>? value)
        {
            if (value == null)
            {
                writer.WriteNullCollectionHeader();
                return;
            }

            var keyFormatter = writer.GetFormatter<TKey>();
            var valueFormatter = writer.GetFormatter<TValue>();

            writer.WriteCollectionHeader(value.Count);
            foreach (var item in value)
            {
                KeyValuePairFormatter.Serialize(keyFormatter, valueFormatter, ref writer, item!);
            }
        }

        [Preserve]
        public override void Deserialize(ref MemoryPackReader reader, scoped ref ImmutableDictionary<TKey, TValue?>? value)
        {
            if (!reader.TryReadCollectionHeader(out var length))
            {
                value = null;
                return;
            }

            if (length == 0)
            {
                value = ImmutableDictionary<TKey, TValue?>.Empty;
                if (keyEqualityComparer != null || valueEqualityComparer != null)
                {
                    value = value.WithComparers(keyEqualityComparer, valueEqualityComparer);
                }
                return;
            }

            var keyFormatter = reader.GetFormatter<TKey>();
            var valueFormatter = reader.GetFormatter<TValue>();

            var builder = ImmutableDictionary.CreateBuilder<TKey, TValue?>(keyEqualityComparer, valueEqualityComparer);
            for (int i = 0; i < length; i++)
            {
                KeyValuePairFormatter.Deserialize(keyFormatter, valueFormatter, ref reader, out var k, out var v);
                builder.Add(k!, v);
            }

            value = builder.ToImmutable();
        }
    }

    [Preserve]
    public sealed class ImmutableHashSetFormatter<T> : MemoryPackFormatter<ImmutableHashSet<T?>>
    {
        readonly IEqualityComparer<T?>? equalityComparer;

        public ImmutableHashSetFormatter()
            : this(null)
        {

        }

        public ImmutableHashSetFormatter(IEqualityComparer<T?>? equalityComparer)
        {
            this.equalityComparer = equalityComparer;
        }

        [Preserve]
        public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref ImmutableHashSet<T?>? value)
        {
            if (value == null)
            {
                writer.WriteNullCollectionHeader();
                return;
            }

            var formatter = writer.GetFormatter<T?>();
            writer.WriteCollectionHeader(value.Count);
            foreach (var item in value)
            {
                var v = item;
                formatter.Serialize(ref writer, ref v);
            }
        }

        [Preserve]
        public override void Deserialize(ref MemoryPackReader reader, scoped ref ImmutableHashSet<T?>? value)
        {
            if (!reader.TryReadCollectionHeader(out var length))
            {
                value = null;
                return;
            }

            if (length == 0)
            {
                value = ImmutableHashSet<T?>.Empty;
                if (equalityComparer != null)
                {
                    value = value.WithComparer(equalityComparer);
                }
                return;
            }

            if (length == 1)
            {
                var item = reader.ReadValue<T>();
                value = ImmutableHashSet.Create(equalityComparer, item);
                return;
            }

            var formatter = reader.GetFormatter<T?>();

            var builder = ImmutableHashSet.CreateBuilder<T?>(equalityComparer);
            for (int i = 0; i < length; i++)
            {
                T? item = default;
                formatter.Deserialize(ref reader, ref item);
                builder.Add(item);
            }

            value = builder.ToImmutable();
        }
    }

    [Preserve]
    public sealed class ImmutableSortedDictionaryFormatter<TKey, TValue> : MemoryPackFormatter<ImmutableSortedDictionary<TKey, TValue?>?>
        where TKey : notnull
    {
        readonly IComparer<TKey>? keyComparer;
        readonly IEqualityComparer<TValue?>? valueEqualityComparer;

        public ImmutableSortedDictionaryFormatter()
            : this(null, null)
        {

        }

        public ImmutableSortedDictionaryFormatter(IComparer<TKey>? keyComparer, IEqualityComparer<TValue?>? valueEqualityComparer)
        {
            this.keyComparer = keyComparer;
            this.valueEqualityComparer = valueEqualityComparer;
        }

        [Preserve]
        public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref ImmutableSortedDictionary<TKey, TValue?>? value)
        {
            if (value == null)
            {
                writer.WriteNullCollectionHeader();
                return;
            }

            var keyFormatter = writer.GetFormatter<TKey>();
            var valueFormatter = writer.GetFormatter<TValue>();

            writer.WriteCollectionHeader(value.Count);
            foreach (var item in value)
            {
                KeyValuePairFormatter.Serialize(keyFormatter, valueFormatter, ref writer, item!);
            }
        }

        [Preserve]
        public override void Deserialize(ref MemoryPackReader reader, scoped ref ImmutableSortedDictionary<TKey, TValue?>? value)
        {
            if (!reader.TryReadCollectionHeader(out var length))
            {
                value = null;
                return;
            }

            if (length == 0)
            {
                value = ImmutableSortedDictionary<TKey, TValue?>.Empty;
                if (keyComparer != null || valueEqualityComparer != null)
                {
                    value = value.WithComparers(keyComparer, valueEqualityComparer);
                }
                return;
            }

            var keyFormatter = reader.GetFormatter<TKey>();
            var valueFormatter = reader.GetFormatter<TValue>();

            var builder = ImmutableSortedDictionary.CreateBuilder<TKey, TValue?>(keyComparer, valueEqualityComparer);
            for (int i = 0; i < length; i++)
            {
                KeyValuePairFormatter.Deserialize(keyFormatter, valueFormatter, ref reader, out var k, out var v);
                builder.Add(k!, v);
            }

            value = builder.ToImmutable();
        }
    }

    [Preserve]
    public sealed class ImmutableSortedSetFormatter<T> : MemoryPackFormatter<ImmutableSortedSet<T?>>
    {
        readonly IComparer<T?>? keyComparer;

        public ImmutableSortedSetFormatter()
            : this(null)
        {

        }

        public ImmutableSortedSetFormatter(IComparer<T?>? keyComparer)
        {
            this.keyComparer = keyComparer;
        }

        [Preserve]
        public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref ImmutableSortedSet<T?>? value)
        {
            if (value == null)
            {
                writer.WriteNullCollectionHeader();
                return;
            }

            var formatter = writer.GetFormatter<T?>();
            writer.WriteCollectionHeader(value.Count);
            foreach (var item in value)
            {
                var v = item;
                formatter.Serialize(ref writer, ref v);
            }
        }

        [Preserve]
        public override void Deserialize(ref MemoryPackReader reader, scoped ref ImmutableSortedSet<T?>? value)
        {
            if (!reader.TryReadCollectionHeader(out var length))
            {
                value = null;
                return;
            }

            if (length == 0)
            {
                value = ImmutableSortedSet<T?>.Empty;
                if (keyComparer != null)
                {
                    value = value.WithComparer(keyComparer);
                }
                return;
            }

            if (length == 1)
            {
                var item = reader.ReadValue<T>();
                value = ImmutableSortedSet.Create(keyComparer, item);
                return;
            }

            var formatter = reader.GetFormatter<T?>();

            var builder = ImmutableSortedSet.CreateBuilder<T?>(keyComparer);
            for (int i = 0; i < length; i++)
            {
                T? item = default;
                formatter.Deserialize(ref reader, ref item);
                builder.Add(item);
            }

            value = builder.ToImmutable();
        }
    }

    [Preserve]
    public sealed class InterfaceImmutableListFormatter<T> : MemoryPackFormatter<IImmutableList<T?>>
    {
        [Preserve]
        public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref IImmutableList<T?>? value)
        {
            if (value == null)
            {
                writer.WriteNullCollectionHeader();
                return;
            }

            var formatter = writer.GetFormatter<T?>();
            writer.WriteCollectionHeader(value.Count);
            foreach (var item in value)
            {
                var v = item;
                formatter.Serialize(ref writer, ref v);
            }
        }

        [Preserve]
        public override void Deserialize(ref MemoryPackReader reader, scoped ref IImmutableList<T?>? value)
        {
            if (!reader.TryReadCollectionHeader(out var length))
            {
                value = null;
                return;
            }

            if (length == 0)
            {
                value = ImmutableList<T?>.Empty;
                return;
            }

            if (length == 1)
            {
                var item = reader.ReadValue<T>();
                value = ImmutableList.Create(item);
                return;
            }

            var formatter = reader.GetFormatter<T?>();

            var builder = ImmutableList.CreateBuilder<T?>();
            for (int i = 0; i < length; i++)
            {
                T? item = default;
                formatter.Deserialize(ref reader, ref item);
                builder.Add(item);
            }

            value = builder.ToImmutable();
        }
    }

    [Preserve]
    public sealed class InterfaceImmutableQueueFormatter<T> : MemoryPackFormatter<IImmutableQueue<T?>>
    {
        [Preserve]
        public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref IImmutableQueue<T?>? value)
        {
            if (value == null)
            {
                writer.WriteNullCollectionHeader();
                return;
            }

            // ImmutableQueue<T> has no Count, so use similar serialization of IEnumerable<T>

            var tempBuffer = ReusableLinkedArrayBufferWriterPool.Rent();
            try
            {
                var tempWriter = new MemoryPackWriter<ReusableLinkedArrayBufferWriter>(ref tempBuffer, writer.OptionalState);

                var count = 0;
                var formatter = writer.GetFormatter<T?>();
                foreach (var item in value)
                {
                    count++;
                    var v = item;
                    formatter.Serialize(ref tempWriter, ref v);
                }

                tempWriter.Flush();

                // write to parameter writer.
                writer.WriteCollectionHeader(count);
                tempBuffer.WriteToAndReset(ref writer);
            }
            finally
            {
                ReusableLinkedArrayBufferWriterPool.Return(tempBuffer);
            }
        }

        [Preserve]
        public override void Deserialize(ref MemoryPackReader reader, scoped ref IImmutableQueue<T?>? value)
        {
            if (!reader.TryReadCollectionHeader(out var length))
            {
                value = null;
                return;
            }

            if (length == 0)
            {
                value = ImmutableQueue<T?>.Empty;
                return;
            }

            if (length == 1)
            {
                var item = reader.ReadValue<T>();
                value = ImmutableQueue.Create(item);
                return;
            }

            // ImmutableQueue<T> has no builder

            var rentArray = ArrayPool<T?>.Shared.Rent(length);
            try
            {
                var formatter = reader.GetFormatter<T?>();
                for (int i = 0; i < length; i++)
                {
                    formatter.Deserialize(ref reader, ref rentArray[i]);
                }

                if (rentArray.Length == length)
                {
                    // we can use T[] ctor
                    value = ImmutableQueue.Create(rentArray);
                    return;
                }
                else
                {
                    // IEnumerable<T> method
                    value = ImmutableQueue.CreateRange((new ArraySegment<T?>(rentArray, 0, length)).AsEnumerable());
                }
            }
            finally
            {
                ArrayPool<T?>.Shared.Return(rentArray, clearArray: RuntimeHelpers.IsReferenceOrContainsReferences<T>());
            }
        }
    }

    [Preserve]
    public sealed class InterfaceImmutableStackFormatter<T> : MemoryPackFormatter<IImmutableStack<T?>>
    {
        [Preserve]
        public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref IImmutableStack<T?>? value)
        {
            if (value == null)
            {
                writer.WriteNullCollectionHeader();
                return;
            }

            // ImmutableStack<T> has no Count, so use similar serialization of IEnumerable<T>

            var tempBuffer = ReusableLinkedArrayBufferWriterPool.Rent();
            try
            {
                var tempWriter = new MemoryPackWriter<ReusableLinkedArrayBufferWriter>(ref tempBuffer, writer.OptionalState);

                var count = 0;
                var formatter = writer.GetFormatter<T?>();
                foreach (var item in value.AsEnumerable().Reverse()) // serialize reverse order
                {
                    count++;
                    var v = item;
                    formatter.Serialize(ref tempWriter, ref v);
                }

                tempWriter.Flush();

                // write to parameter writer.
                writer.WriteCollectionHeader(count);
                tempBuffer.WriteToAndReset(ref writer);
            }
            finally
            {
                ReusableLinkedArrayBufferWriterPool.Return(tempBuffer);
            }
        }

        [Preserve]
        public override void Deserialize(ref MemoryPackReader reader, scoped ref IImmutableStack<T?>? value)
        {
            if (!reader.TryReadCollectionHeader(out var length))
            {
                value = null;
                return;
            }

            if (length == 0)
            {
                value = ImmutableStack<T?>.Empty;
                return;
            }

            if (length == 1)
            {
                var item = reader.ReadValue<T>();
                value = ImmutableStack.Create(item);
                return;
            }

            // ImmutableStack<T> has no builder

            var rentArray = ArrayPool<T?>.Shared.Rent(length);
            try
            {
                var formatter = reader.GetFormatter<T?>();
                for (int i = 0; i < length; i++)
                {
                    formatter.Deserialize(ref reader, ref rentArray[i]);
                }

                if (rentArray.Length == length)
                {
                    // we can use T[] ctor
                    value = ImmutableStack.Create(rentArray);
                    return;
                }
                else
                {
                    // IEnumerable<T> method
                    value = ImmutableStack.CreateRange((new ArraySegment<T?>(rentArray, 0, length)).AsEnumerable());
                }
            }
            finally
            {
                ArrayPool<T?>.Shared.Return(rentArray, clearArray: RuntimeHelpers.IsReferenceOrContainsReferences<T>());
            }
        }
    }

    [Preserve]
    public sealed class InterfaceImmutableDictionaryFormatter<TKey, TValue> : MemoryPackFormatter<IImmutableDictionary<TKey, TValue?>?>
        where TKey : notnull
    {
        readonly IEqualityComparer<TKey>? keyEqualityComparer;
        readonly IEqualityComparer<TValue?>? valueEqualityComparer;

        public InterfaceImmutableDictionaryFormatter()
            : this(null, null)
        {

        }

        public InterfaceImmutableDictionaryFormatter(IEqualityComparer<TKey>? keyEqualityComparer, IEqualityComparer<TValue?>? valueEqualityComparer)
        {
            this.keyEqualityComparer = keyEqualityComparer;
            this.valueEqualityComparer = valueEqualityComparer;
        }

        [Preserve]
        public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref IImmutableDictionary<TKey, TValue?>? value)
        {
            if (value == null)
            {
                writer.WriteNullCollectionHeader();
                return;
            }

            var keyFormatter = writer.GetFormatter<TKey>();
            var valueFormatter = writer.GetFormatter<TValue>();

            writer.WriteCollectionHeader(value.Count);
            foreach (var item in value)
            {
                KeyValuePairFormatter.Serialize(keyFormatter, valueFormatter, ref writer, item!);
            }
        }

        [Preserve]
        public override void Deserialize(ref MemoryPackReader reader, scoped ref IImmutableDictionary<TKey, TValue?>? value)
        {
            if (!reader.TryReadCollectionHeader(out var length))
            {
                value = null;
                return;
            }

            if (length == 0)
            {
                if (keyEqualityComparer != null || valueEqualityComparer != null)
                {
                    value = ImmutableDictionary<TKey, TValue?>.Empty.WithComparers(keyEqualityComparer, valueEqualityComparer);
                }
                else
                {
                    value = ImmutableDictionary<TKey, TValue?>.Empty;
                }
                return;
            }

            var keyFormatter = reader.GetFormatter<TKey>();
            var valueFormatter = reader.GetFormatter<TValue>();

            var builder = ImmutableDictionary.CreateBuilder<TKey, TValue?>(keyEqualityComparer, valueEqualityComparer);
            for (int i = 0; i < length; i++)
            {
                KeyValuePairFormatter.Deserialize(keyFormatter, valueFormatter, ref reader, out var k, out var v);
                builder.Add(k!, v);
            }

            value = builder.ToImmutable();
        }
    }

    [Preserve]
    public sealed class InterfaceImmutableSetFormatter<T> : MemoryPackFormatter<IImmutableSet<T?>>
    {
        readonly IEqualityComparer<T?>? equalityComparer;

        public InterfaceImmutableSetFormatter()
            : this(null)
        {

        }

        public InterfaceImmutableSetFormatter(IEqualityComparer<T?>? equalityComparer)
        {
            this.equalityComparer = equalityComparer;
        }

        [Preserve]
        public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref IImmutableSet<T?>? value)
        {
            if (value == null)
            {
                writer.WriteNullCollectionHeader();
                return;
            }

            var formatter = writer.GetFormatter<T?>();
            writer.WriteCollectionHeader(value.Count);
            foreach (var item in value)
            {
                var v = item;
                formatter.Serialize(ref writer, ref v);
            }
        }

        [Preserve]
        public override void Deserialize(ref MemoryPackReader reader, scoped ref IImmutableSet<T?>? value)
        {
            if (!reader.TryReadCollectionHeader(out var length))
            {
                value = null;
                return;
            }

            if (length == 0)
            {
                if (equalityComparer != null)
                {
                    value = ImmutableHashSet<T?>.Empty.WithComparer(equalityComparer);
                }
                else
                {
                    value = ImmutableHashSet<T?>.Empty;
                }
                return;
            }

            if (length == 1)
            {
                var item = reader.ReadValue<T>();
                value = ImmutableHashSet.Create(equalityComparer, item);
                return;
            }

            var formatter = reader.GetFormatter<T?>();

            var builder = ImmutableHashSet.CreateBuilder<T?>(equalityComparer);
            for (int i = 0; i < length; i++)
            {
                T? item = default;
                formatter.Deserialize(ref reader, ref item);
                builder.Add(item);
            }

            value = builder.ToImmutable();
        }
    }
}
