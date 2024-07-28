using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;

namespace FFramework
{
    public struct DynamicSequence : IDisposable 
    {
        public int WritePosition { get; set; }
        
        public int WriteIndex { get; set; }

        public int ReadIndex { get; set; }

        public int ReadPosition { get; set; }

        private List<ArrayBuffer<byte>> m_Buffers;

        private const int MinSize = 1024;

        /// <summary>
        /// 写入移动
        /// </summary>
        /// <param name="length">长度</param>
        /// <param name="writePosition">应当写入的位置</param>
        public void WriteMove(int length,out Span<byte> writePosition)
        {
            //写入初始化
            m_Buffers ??= Envirment.Current.GetModule<PoolModule>().Get< List<ArrayBuffer<byte>>, ListPoolable<List<ArrayBuffer<byte>> >>();
            if (m_Buffers.Count == 0)
            {
                int blockCount = length / MinSize + 1;
                int bufferSize = MinSize * blockCount;
                m_Buffers.Add(new ArrayBuffer<byte>(ArrayPool<byte>.Shared.Rent(bufferSize),0));
                WritePosition = 0;
                WriteIndex = 0;
            }

            //超出范围
            if(WritePosition + length > m_Buffers[WriteIndex].Buffer.Length)
            {
                int blockCount = length / MinSize + 1;
                int bufferSize = MinSize * blockCount;
                m_Buffers.Add(new ArrayBuffer<byte>(ArrayPool<byte>.Shared.Rent(bufferSize),0));
                WritePosition = 0;
                WriteIndex++;

                WritePosition += length;

                //上一个完全没利用，回收
                int lastIndex = WriteIndex - 1;
                if (m_Buffers[lastIndex].InvalidLength==0)
                {
                    ArrayPool<byte>.Shared.Return(m_Buffers[lastIndex].Buffer);
                    m_Buffers.RemoveAt(lastIndex);
                    --WriteIndex;
                }
            }
            else
            {
                WritePosition += length;
                m_Buffers[WriteIndex] = m_Buffers[WriteIndex].SetInvalidLength(WritePosition);
            }

            writePosition =  m_Buffers[WriteIndex].Buffer.AsSpan(WritePosition - length,length);
        }

        public void ReadMove(int length,out ReadOnlySpan<byte> readPosition)
        {
            ReadPosition += length;
            if (ReadPosition > m_Buffers[ReadIndex].InvalidLength)
            {
                ReadPosition = length;
                ReadIndex++;

                if(m_Buffers.Count<= ReadIndex)
                {
                    throw new IndexOutOfRangeException("End of Stream");
                }
            }
            readPosition =  m_Buffers[ReadIndex].Buffer.AsSpan(ReadPosition - length,length);
        }

        public void Dispose()
        {
            Envirment.Current.GetModule<PoolModule>().Set(m_Buffers);
            m_Buffers = null;
        }

        public byte[] GetMergeSequence()
        {
            int total = 0;
            for(int i=0;i< m_Buffers.Count;i++)
            {
                total += m_Buffers[i].InvalidLength;
            }
 
            byte[] buffer = new byte[total];
            total = 0;
            for (int i = 0; i < m_Buffers.Count; i++)
            {
                if (m_Buffers[i].InvalidLength == 0) continue;
                var writeSpan = new Span<byte>(buffer,total,buffer.Length-total);
                m_Buffers[i].Buffer.AsSpan(0, m_Buffers[i].InvalidLength).CopyTo(buffer);
                total += m_Buffers[i].InvalidLength;
            }

            return buffer;
        }

       
    }
}
