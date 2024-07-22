using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace FFramework
{
    internal static class VarIntTypeCode
    {
        public const byte MaxSingleValue = 127;
        public const sbyte MinSingleValue = -120;

        public const sbyte Byte = -121;
        public const sbyte SByte = -122;
        public const sbyte UInt16 = -123;
        public const sbyte Int16 = -124;
        public const sbyte UInt32 = -125;
        public const sbyte Int32 = -126;
        public const sbyte UInt64 = -127;
        public const sbyte Int64 = -128;
    }


    public sealed partial class VarIntReadWriteHelper
    {
        private static int TypeCodeSize;
        static VarIntReadWriteHelper()
        {
            TypeCodeSize = Unsafe.SizeOf<sbyte>();
        }

        public bool Write(Byte value, Span<byte> dst)
        {
            if (value <= VarIntTypeCode.MaxSingleValue)
            {
                bool writeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                    .Write(dst, (sbyte)value);
                return writeResult;
            }
            else
            {
                bool writeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                    .Write(dst, VarIntTypeCode.Byte);

                if (!writeResult) return writeResult;

                Span<byte> dataDst = dst.Slice(TypeCodeSize);
                writeResult = ThreadSingletonProperty<UInt8ReadWriteProvider>.Instance
                    .Write(dataDst, value);

                return writeResult;
            }
        }


        public bool Write(SByte value, Span<byte> dst)
        {
            if (VarIntTypeCode.MinSingleValue <= value)
            {
                bool writeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                    .Write(dst, value);

                return writeResult;
            }
            else
            {
                bool writeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                    .Write(dst, VarIntTypeCode.SByte);

                if (!writeResult) return writeResult;

                Span<byte> dataDst = dst.Slice(TypeCodeSize);
                writeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                    .Write(dataDst, value);

                return writeResult;
            }
        }

        public bool Write(Int16 value, Span<byte> dst)
        {
            if (VarIntTypeCode.MinSingleValue <= value)
            {
                bool writeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                    .Write(dst, (sbyte)value);
                return writeResult;
            }
            else
            {
                bool writeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                    .Write(dst, VarIntTypeCode.Int16);

                if (!writeResult) return writeResult;

                Span<byte> dataDst = dst.Slice(TypeCodeSize);
                ThreadSingletonProperty<Int16ReadWriteProvider>.Instance
                    .Write(dataDst, value);
                return writeResult;
            }
        }

        public bool Write(UInt16 value, Span<byte> dst)
        {
            if (value <= VarIntTypeCode.MaxSingleValue)
            {
                bool writeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                    .Write(dst, (sbyte)value);
                return writeResult;
            }
            else
            {
                bool writeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                    .Write(dst, VarIntTypeCode.UInt16);

                if (!writeResult) return writeResult;

                Span<byte> dataDst = dst.Slice(TypeCodeSize);
                writeResult = ThreadSingletonProperty<UInt16ReadWriteProvider>.Instance
                    .Write(dataDst, value);

                return writeResult;

            }
        }

        public bool Write(Int32 value, Span<byte> dst)
        {
            bool writeResult;

            if (0 <= value)
            {
                if (value <= VarIntTypeCode.MaxSingleValue)
                {
                    writeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                        .Write(dst, (sbyte)value);
                    return writeResult;
                }
                else if (value <= Int16.MaxValue)
                {
                    writeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                        .Write(dst, VarIntTypeCode.Int16);

                    if (!writeResult) return writeResult;

                    Span<byte> dataDst = dst.Slice(TypeCodeSize);
                    writeResult = ThreadSingletonProperty<Int16ReadWriteProvider>.Instance
                        .Write(dataDst, (Int16)value);

                    return writeResult;
                }
                else
                {
                    writeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                        .Write(dst, VarIntTypeCode.Int32);

                    if (!writeResult) return writeResult;

                    Span<byte> dataDst = dst.Slice(TypeCodeSize);
                    writeResult = ThreadSingletonProperty<Int32ReadWriteProvider>.Instance
                        .Write(dataDst, (Int32)value);

                    return writeResult;
                }
            }
            else
            {
                if (VarIntTypeCode.MinSingleValue <= value)
                {
                    writeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                        .Write(dst, (sbyte)value);
                    return writeResult;
                }
                else if (sbyte.MinValue <= value)
                {
                    writeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                        .Write(dst, VarIntTypeCode.SByte);

                    if (!writeResult) return writeResult;

                    Span<byte> dataDst = dst.Slice(TypeCodeSize);
                    writeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                        .Write(dataDst, (SByte)value);

                    return writeResult;
                }
                else if (short.MinValue <= value)
                {
                    writeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                        .Write(dst, VarIntTypeCode.Int16);

                    if (!writeResult) return writeResult;

                    Span<byte> dataDst = dst.Slice(TypeCodeSize);
                    writeResult = ThreadSingletonProperty<Int16ReadWriteProvider>.Instance
                        .Write(dataDst, (Int16)value);

                    return writeResult;
                }
                else
                {
                    writeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                        .Write(dst, VarIntTypeCode.Int32);

                    if (!writeResult) return writeResult;

                    Span<byte> dataDst = dst.Slice(TypeCodeSize);
                    writeResult = ThreadSingletonProperty<Int32ReadWriteProvider>.Instance
                        .Write(dataDst, (Int32)value);

                    return writeResult;
                }
            }
        }


        public bool Write(UInt32 value, Span<byte> dst)
        {
            bool writeResult;

            if (value <= VarIntTypeCode.MaxSingleValue)
            {
                writeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                    .Write(dst, (sbyte)value);
                return writeResult;
            }
            else if (value <= UInt16.MaxValue)
            {
                writeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                    .Write(dst, VarIntTypeCode.UInt16);

                if (!writeResult) return writeResult;

                Span<byte> dataDst = dst.Slice(TypeCodeSize);
                writeResult = ThreadSingletonProperty<UInt16ReadWriteProvider>.Instance
                    .Write(dataDst, (UInt16)value);

                return writeResult;
            }
            else
            {
                writeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                    .Write(dst, VarIntTypeCode.UInt32);

                if (!writeResult) return writeResult;

                Span<byte> dataDst = dst.Slice(TypeCodeSize);
                writeResult = ThreadSingletonProperty<UInt32ReadWriteProvider>.Instance
                    .Write(dataDst, value);

                return writeResult;
            }
        }


        public bool Write(Int64 value, Span<byte> dst)
        {
            bool writeResult;

            if (0 <= value)
            {
                if (value <= VarIntTypeCode.MaxSingleValue)
                {
                    // Write as sbyte
                    writeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                        .Write(dst, (sbyte)value);
                    return writeResult;
                }
                else if (value <= Int16.MaxValue)
                {
                    // Write type code and value as Int16
                    writeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                        .Write(dst, VarIntTypeCode.Int16);

                    if (!writeResult) return writeResult;

                    Span<byte> dataDst = dst.Slice(TypeCodeSize);
                    writeResult = ThreadSingletonProperty<Int16ReadWriteProvider>.Instance
                        .Write(dataDst, (Int16)value);

                    return writeResult;
                }
                else if (value <= Int32.MaxValue)
                {
                    // Write type code and value as Int32
                    writeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                        .Write(dst, VarIntTypeCode.Int32);

                    if (!writeResult) return writeResult;

                    Span<byte> dataDst = dst.Slice(TypeCodeSize);
                    writeResult = ThreadSingletonProperty<Int32ReadWriteProvider>.Instance
                        .Write(dataDst, (Int32)value);

                    return writeResult;
                }
                else
                {
                    // Write type code and value as Int64
                    writeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                        .Write(dst, VarIntTypeCode.Int64);

                    if (!writeResult) return writeResult;

                    Span<byte> dataDst = dst.Slice(TypeCodeSize);
                    writeResult = ThreadSingletonProperty<Int64ReadWriteProvider>.Instance
                        .Write(dataDst, value);

                    return writeResult;
                }
            }
            else
            {
                if (VarIntTypeCode.MinSingleValue <= value)
                {
                    // Write as sbyte
                    writeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                        .Write(dst, (sbyte)value);
                    return writeResult;
                }
                else if (sbyte.MinValue <= value)
                {
                    // Write type code and value as SByte
                    writeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                        .Write(dst, VarIntTypeCode.SByte);

                    if (!writeResult) return writeResult;

                    Span<byte> dataDst = dst.Slice(TypeCodeSize);
                    writeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                        .Write(dataDst, (SByte)value);

                    return writeResult;
                }
                else if (short.MinValue <= value)
                {
                    // Write type code and value as Int16
                    writeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                        .Write(dst, VarIntTypeCode.Int16);

                    if (!writeResult) return writeResult;

                    Span<byte> dataDst = dst.Slice(TypeCodeSize);
                    writeResult = ThreadSingletonProperty<Int16ReadWriteProvider>.Instance
                        .Write(dataDst, (Int16)value);

                    return writeResult;
                }
                else if (int.MinValue <= value)
                {
                    // Write type code and value as Int32
                    writeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                        .Write(dst, VarIntTypeCode.Int32);

                    if (!writeResult) return writeResult;

                    Span<byte> dataDst = dst.Slice(TypeCodeSize);
                    writeResult = ThreadSingletonProperty<Int32ReadWriteProvider>.Instance
                        .Write(dataDst, (Int32)value);

                    return writeResult;
                }
                else
                {
                    // Write type code and value as Int64
                    writeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                        .Write(dst, VarIntTypeCode.Int64);

                    if (!writeResult) return writeResult;

                    Span<byte> dataDst = dst.Slice(TypeCodeSize);
                    writeResult = ThreadSingletonProperty<Int64ReadWriteProvider>.Instance
                        .Write(dataDst, value);

                    return writeResult;
                }
            }
        }


        public bool Write(UInt64 value, Span<byte> dst)
        {
            bool writeResult;

            if (value <= VarIntTypeCode.MaxSingleValue)
            {
                // Write as sbyte
                writeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                    .Write(dst, (sbyte)value);
                return writeResult;
            }
            else if (value <= UInt16.MaxValue)
            {
                // Write type code and value as UInt16
                writeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                    .Write(dst, VarIntTypeCode.UInt16);

                if (!writeResult) return writeResult;

                Span<byte> dataDst = dst.Slice(TypeCodeSize);
                writeResult = ThreadSingletonProperty<UInt16ReadWriteProvider>.Instance
                    .Write(dataDst, (UInt16)value);

                return writeResult;
            }
            else if (value <= UInt32.MaxValue)
            {
                // Write type code and value as UInt32
                writeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                    .Write(dst, VarIntTypeCode.UInt32);

                if (!writeResult) return writeResult;

                Span<byte> dataDst = dst.Slice(TypeCodeSize);
                writeResult = ThreadSingletonProperty<UInt32ReadWriteProvider>.Instance
                    .Write(dataDst, (UInt32)value);

                return writeResult;
            }
            else
            {
                // Write type code and value as UInt64
                writeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                    .Write(dst, VarIntTypeCode.UInt64);

                if (!writeResult) return writeResult;

                Span<byte> dataDst = dst.Slice(TypeCodeSize);
                writeResult = ThreadSingletonProperty<UInt64ReadWriteProvider>.Instance
                    .Write(dataDst, value);

                return writeResult;
            }
        }




        public bool ReadByte(ReadOnlySpan<byte> src, out Byte value)
        {
            bool readTypeCodeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                .Read(src, out sbyte typeCode);

            if (!readTypeCodeResult)
            {
                value = default;
                return false;
            }

            switch (typeCode)
            {
                case VarIntTypeCode.Byte:
                    bool readByteResult = ThreadSingletonProperty<UInt8ReadWriteProvider>.Instance
                        .Read(src.Slice(TypeCodeSize), out byte byteValue);
                    if (readByteResult)
                    {
                        value = byteValue;
                        return true;
                    }
                    break;

                case VarIntTypeCode.SByte:
                    bool readSByteResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                        .Read(src.Slice(TypeCodeSize), out sbyte sbyteValue);
                    if (readSByteResult)
                    {
                        value = (byte)sbyteValue;
                        return true;
                    }
                    break;

                case VarIntTypeCode.UInt16:
                    bool readUInt16Result = ThreadSingletonProperty<UInt16ReadWriteProvider>.Instance
                        .Read(src.Slice(TypeCodeSize), out ushort uint16Value);
                    if (readUInt16Result)
                    {
                        if (uint16Value <= byte.MaxValue)
                        {
                            value = (byte)uint16Value;
                            return true;
                        }
                    }
                    break;

                case VarIntTypeCode.UInt32:
                    bool readUInt32Result = ThreadSingletonProperty<UInt32ReadWriteProvider>.Instance
                        .Read(src.Slice(TypeCodeSize), out uint uint32Value);
                    if (readUInt32Result)
                    {
                        if (uint32Value <= byte.MaxValue)
                        {
                            value = (byte)uint32Value;
                            return true;
                        }
                    }
                    break;

                case VarIntTypeCode.UInt64:
                    bool readUInt64Result = ThreadSingletonProperty<UInt64ReadWriteProvider>.Instance
                        .Read(src.Slice(TypeCodeSize), out ulong uint64Value);
                    if (readUInt64Result)
                    {
                        if (uint64Value <= byte.MaxValue)
                        {
                            value = (byte)uint64Value;
                            return true;
                        }
                    }
                    break;

                case VarIntTypeCode.Int16:
                    bool readInt16Result = ThreadSingletonProperty<Int16ReadWriteProvider>.Instance
                        .Read(src.Slice(TypeCodeSize), out short int16Value);
                    if (readInt16Result)
                    {
                        if (int16Value >= byte.MinValue && int16Value <= byte.MaxValue)
                        {
                            value = (byte)int16Value;
                            return true;
                        }
                    }
                    break;

                case VarIntTypeCode.Int32:
                    bool readInt32Result = ThreadSingletonProperty<Int32ReadWriteProvider>.Instance
                        .Read(src.Slice(TypeCodeSize), out int int32Value);
                    if (readInt32Result)
                    {
                        if (int32Value >= byte.MinValue && int32Value <= byte.MaxValue)
                        {
                            value = (byte)int32Value;
                            return true;
                        }
                    }
                    break;

                case VarIntTypeCode.Int64:
                    bool readInt64Result = ThreadSingletonProperty<Int64ReadWriteProvider>.Instance
                        .Read(src.Slice(TypeCodeSize), out long int64Value);
                    if (readInt64Result)
                    {
                        if (int64Value >= byte.MinValue && int64Value <= byte.MaxValue)
                        {
                            value = (byte)int64Value;
                            return true;
                        }
                    }
                    break;

                default:
                    if (typeCode >= 0)
                    {
                        value = (byte)typeCode;
                        return true;
                    }
                    break;
            }

            value = default;
            return false;
        }

        public bool ReadUInt16(ReadOnlySpan<byte> src, out UInt16 value)
        {
            bool readTypeCodeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                .Read(src, out sbyte typeCode);

            if (!readTypeCodeResult)
            {
                value = default;
                return false;
            }

            switch (typeCode)
            {
                case VarIntTypeCode.Byte:
                    bool readByteResult = ThreadSingletonProperty<UInt8ReadWriteProvider>.Instance
                        .Read(src.Slice(TypeCodeSize), out byte byteValue);
                    if (readByteResult)
                    {
                        value = byteValue;
                        return true;
                    }
                    break;

                case VarIntTypeCode.UInt16:
                    bool readUInt16Result = ThreadSingletonProperty<UInt16ReadWriteProvider>.Instance
                        .Read(src.Slice(TypeCodeSize), out ushort uint16Value);
                    if (readUInt16Result)
                    {
                        value = uint16Value;
                        return true;
                    }
                    break;

                case VarIntTypeCode.UInt32:
                    bool readUInt32Result = ThreadSingletonProperty<UInt32ReadWriteProvider>.Instance
                        .Read(src.Slice(TypeCodeSize), out uint uint32Value);
                    if (readUInt32Result)
                    {
                        if (uint32Value <= ushort.MaxValue)
                        {
                            value = (ushort)uint32Value;
                            return true;
                        }
                    }
                    break;

                case VarIntTypeCode.UInt64:
                    bool readUInt64Result = ThreadSingletonProperty<UInt64ReadWriteProvider>.Instance
                        .Read(src.Slice(TypeCodeSize), out ulong uint64Value);
                    if (readUInt64Result)
                    {
                        if (uint64Value <= ushort.MaxValue)
                        {
                            value = (ushort)uint64Value;
                            return true;
                        }
                    }
                    break;

                case VarIntTypeCode.Int16:
                    bool readInt16Result = ThreadSingletonProperty<Int16ReadWriteProvider>.Instance
                        .Read(src.Slice(TypeCodeSize), out short int16Value);
                    if (readInt16Result)
                    {
                        if (int16Value >= 0 && int16Value <= ushort.MaxValue)
                        {
                            value = (ushort)int16Value;
                            return true;
                        }
                    }
                    break;

                case VarIntTypeCode.Int32:
                    bool readInt32Result = ThreadSingletonProperty<Int32ReadWriteProvider>.Instance
                        .Read(src.Slice(TypeCodeSize), out int int32Value);
                    if (readInt32Result)
                    {
                        if (int32Value >= 0 && int32Value <= ushort.MaxValue)
                        {
                            value = (ushort)int32Value;
                            return true;
                        }
                    }
                    break;

                case VarIntTypeCode.Int64:
                    bool readInt64Result = ThreadSingletonProperty<Int64ReadWriteProvider>.Instance
                        .Read(src.Slice(TypeCodeSize), out long int64Value);
                    if (readInt64Result)
                    {
                        if (int64Value >= 0 && int64Value <= ushort.MaxValue)
                        {
                            value = (ushort)int64Value;
                            return true;
                        }
                    }
                    break;

                default:
                    if (typeCode >= 0 && typeCode <= ushort.MaxValue)
                    {
                        value = (ushort)typeCode;
                        return true;
                    }
                    break;
            }

            value = default;
            return false;
        }


        public bool ReadInt16(ReadOnlySpan<byte> src, out Int16 value)
        {
            bool readTypeCodeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                .Read(src, out sbyte typeCode);

            if (!readTypeCodeResult)
            {
                value = default;
                return false;
            }

            switch (typeCode)
            {
                case VarIntTypeCode.Byte:
                    bool readByteResult = ThreadSingletonProperty<UInt8ReadWriteProvider>.Instance
                        .Read(src.Slice(TypeCodeSize), out byte byteValue);
                    if (readByteResult)
                    {
                        value = byteValue;
                        return true;
                    }
                    break;

                case VarIntTypeCode.SByte:
                    bool readSByteResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                        .Read(src.Slice(TypeCodeSize), out sbyte sbyteValue);
                    if (readSByteResult)
                    {
                        value = sbyteValue;
                        return true;
                    }
                    break;

                case VarIntTypeCode.Int16:
                    bool readInt16Result = ThreadSingletonProperty<Int16ReadWriteProvider>.Instance
                        .Read(src.Slice(TypeCodeSize), out short int16Value);
                    if (readInt16Result)
                    {
                        value = int16Value;
                        return true;
                    }
                    break;

                case VarIntTypeCode.Int32:
                    bool readInt32Result = ThreadSingletonProperty<Int32ReadWriteProvider>.Instance
                        .Read(src.Slice(TypeCodeSize), out int int32Value);
                    if (readInt32Result)
                    {
                        if (int32Value >= short.MinValue && int32Value <= short.MaxValue)
                        {
                            value = (short)int32Value;
                            return true;
                        }
                    }
                    break;

                case VarIntTypeCode.Int64:
                    bool readInt64Result = ThreadSingletonProperty<Int64ReadWriteProvider>.Instance
                        .Read(src.Slice(TypeCodeSize), out long int64Value);
                    if (readInt64Result)
                    {
                        if (int64Value >= short.MinValue && int64Value <= short.MaxValue)
                        {
                            value = (short)int64Value;
                            return true;
                        }
                    }
                    break;

                default:
                    if (typeCode >= short.MinValue && typeCode <= short.MaxValue)
                    {
                        value = (short)typeCode;
                        return true;
                    }
                    break;
            }

            value = default;
            return false;
        }
        public bool ReadUInt32(ReadOnlySpan<byte> src, out UInt32 value)
        {
            bool readTypeCodeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                .Read(src, out sbyte typeCode);

            if (!readTypeCodeResult)
            {
                value = default;
                return false;
            }

            switch (typeCode)
            {
                case VarIntTypeCode.Byte:
                    bool readByteResult = ThreadSingletonProperty<UInt8ReadWriteProvider>.Instance
                        .Read(src.Slice(TypeCodeSize), out byte byteValue);
                    if (readByteResult)
                    {
                        value = byteValue;
                        return true;
                    }
                    break;

                case VarIntTypeCode.UInt16:
                    bool readUInt16Result = ThreadSingletonProperty<UInt16ReadWriteProvider>.Instance
                        .Read(src.Slice(TypeCodeSize), out ushort uint16Value);
                    if (readUInt16Result)
                    {
                        value = uint16Value;
                        return true;
                    }
                    break;

                case VarIntTypeCode.UInt32:
                    bool readUInt32Result = ThreadSingletonProperty<UInt32ReadWriteProvider>.Instance
                        .Read(src.Slice(TypeCodeSize), out uint uint32Value);
                    if (readUInt32Result)
                    {
                        value = uint32Value;
                        return true;
                    }
                    break;

                case VarIntTypeCode.UInt64:
                    bool readUInt64Result = ThreadSingletonProperty<UInt64ReadWriteProvider>.Instance
                        .Read(src.Slice(TypeCodeSize), out ulong uint64Value);
                    if (readUInt64Result)
                    {
                        if (uint64Value <= uint.MaxValue)
                        {
                            value = (uint)uint64Value;
                            return true;
                        }
                    }
                    break;

                case VarIntTypeCode.Int16:
                    bool readInt16Result = ThreadSingletonProperty<Int16ReadWriteProvider>.Instance
                        .Read(src.Slice(TypeCodeSize), out short int16Value);
                    if (readInt16Result)
                    {
                        if (int16Value >= 0)
                        {
                            value = (uint)int16Value;
                            return true;
                        }
                    }
                    break;

                case VarIntTypeCode.Int32:
                    bool readInt32Result = ThreadSingletonProperty<Int32ReadWriteProvider>.Instance
                        .Read(src.Slice(TypeCodeSize), out int int32Value);
                    if (readInt32Result)
                    {
                        if (int32Value >= 0)
                        {
                            value = (uint)int32Value;
                            return true;
                        }
                    }
                    break;

                case VarIntTypeCode.Int64:
                    bool readInt64Result = ThreadSingletonProperty<Int64ReadWriteProvider>.Instance
                        .Read(src.Slice(TypeCodeSize), out long int64Value);
                    if (readInt64Result)
                    {
                        if (int64Value >= 0 && int64Value <= uint.MaxValue)
                        {
                            value = (uint)int64Value;
                            return true;
                        }
                    }
                    break;

                default:
                    if (typeCode >= 0)
                    {
                        value = (uint)typeCode;
                        return true;
                    }
                    break;
            }

            value = default;
            return false;
        }
        public bool ReadInt32(ReadOnlySpan<byte> src, out Int32 value)
        {
            bool readTypeCodeResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                .Read(src, out sbyte typeCode);

            if (!readTypeCodeResult)
            {
                value = default;
                return false;
            }

            switch (typeCode)
            {
                case VarIntTypeCode.Byte:
                    bool readByteResult = ThreadSingletonProperty<UInt8ReadWriteProvider>.Instance
                        .Read(src.Slice(TypeCodeSize), out byte byteValue);
                    if (readByteResult)
                    {
                        value = byteValue;
                        return true;
                    }
                    break;

                case VarIntTypeCode.SByte:
                    bool readSByteResult = ThreadSingletonProperty<Int8ReadWriteProvider>.Instance
                        .Read(src.Slice(TypeCodeSize), out sbyte sbyteValue);
                    if (readSByteResult)
                    {
                        value = sbyteValue;
                        return true;
                    }
                    break;

                case VarIntTypeCode.Int16:
                    bool readInt16Result = ThreadSingletonProperty<Int16ReadWriteProvider>.Instance
                        .Read(src.Slice(TypeCodeSize), out short int16Value);
                    if (readInt16Result)
                    {
                        value = int16Value;
                        return true;
                    }
                    break;

                case VarIntTypeCode.Int32:
                    bool readInt32Result = ThreadSingletonProperty<Int32ReadWriteProvider>.Instance
                        .Read(src.Slice(TypeCodeSize), out int int32Value);
                    if (readInt32Result)
                    {
                        value = int32Value;
                        return true;
                    }
                    break;


                case VarIntTypeCode.UInt16:
                    bool readUInt16Result = ThreadSingletonProperty<UInt16ReadWriteProvider>.Instance
                        .Read(src.Slice(TypeCodeSize), out ushort uint16Value);
                    if (readUInt16Result)
                    {
                        if (uint16Value >= int.MinValue && uint16Value <= int.MaxValue)
                        {
                            value = (int)uint16Value;
                            return true;
                        }
                    }
                    break;


                default:
                    if (typeCode >= int.MinValue && typeCode <= int.MaxValue)
                    {
                        value = (int)typeCode;
                        return true;
                    }
                    break;
            }

            value = default;
            return false;
        }

    }
}
