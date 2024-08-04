using System.Collections.Generic;

namespace FFramework.Serialization.Binary
{
    public static class CollectionReadWriteUtil
    {
        public static void ReadCollection<T>(ref DynamicSequence sequence, ICollection<T> collection)
        {
            int length = VarIntReadWriteHelper.ReadInt32(ref sequence);
            for (int i = 0; i < length; i++)
            {
                collection.Add(ReadWiteHelper.Read<T>(ref sequence));
            }
        }

        public static void WriteCollection<T>(ref DynamicSequence sequence, ICollection<T> collection)
        {
            VarIntReadWriteHelper.Write(ref sequence, collection.Count);
            foreach (var element in collection)
            {
                ReadWiteHelper.Write(ref sequence, element);
            }
        }



        public static ArrayWrapper<T> ReadCollection<T>(ref DynamicSequence sequence)
        {
            int length = VarIntReadWriteHelper.ReadInt32(ref sequence);
            var array = Envirment.Current.GetModule<PoolModule>().Get<ArrayWrapper<T>,ArrayWrapper<T>.Poolable>();
            array.Preserve(length);
            for (int i = 0; i < length; i++)
            {
                array[i] = ReadWiteHelper.Read<T>(ref sequence);
            }
            return array;
        }
        public static void WriteCollection<T>(ref DynamicSequence sequence, T[] collection)
        {
            VarIntReadWriteHelper.Write(ref sequence, collection.Length);
            for (int i = 0; i < collection.Length; i++)
            {
                ReadWiteHelper.Write(ref sequence, collection[i]);
            }
        }
    }
}
