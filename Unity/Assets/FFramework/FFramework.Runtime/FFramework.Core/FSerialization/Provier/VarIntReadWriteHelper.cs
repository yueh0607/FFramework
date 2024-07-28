using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace FFramework.Serialization.Binary
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


    public static partial class VarIntReadWriteHelper
    {
        private static int TypeCodeSize;
        static VarIntReadWriteHelper()
        {
            TypeCodeSize = Unsafe.SizeOf<sbyte>();
        }
        static void WriteTypeCode(ref DynamicSequence sequence, SByte typeCode)
        {
            ReadWriteUtil.Write<SByte>(ref sequence, typeCode);
        }

        public static void Write(ref DynamicSequence dst, Byte value)
        {
            if (value <= VarIntTypeCode.MaxSingleValue)
            {
                ReadWriteUtil.Write<SByte>(ref dst, (SByte)value);
            }
            else
            {
                WriteTypeCode(ref dst, VarIntTypeCode.Byte);
                ReadWriteUtil.Write<Byte>(ref dst, value);
            }
        }

        public static void Write(ref DynamicSequence dst, SByte value)
        {
            if (VarIntTypeCode.MinSingleValue <= value)
            {
                ReadWriteUtil.Write<SByte>(ref dst, value);
            }
            else
            {
                WriteTypeCode(ref dst, VarIntTypeCode.SByte);
                ReadWriteUtil.Write<SByte>(ref dst, value);
            }
        }


        public static void Write(ref DynamicSequence dst, Int16 value)
        {
            if (VarIntTypeCode.MinSingleValue <= value)
            {
                ReadWriteUtil.Write<SByte>(ref dst, (sbyte)value);
            }
            else
            {
                WriteTypeCode(ref dst, VarIntTypeCode.Int16);
                ReadWriteUtil.Write<Int16>(ref dst, value);
            }
        }

        public static void Write(ref DynamicSequence dst, UInt16 value)
        {
            if (value <= VarIntTypeCode.MaxSingleValue)
            {
                ReadWriteUtil.Write<SByte>(ref dst, (sbyte)value);
            }
            else
            {
                WriteTypeCode(ref dst, VarIntTypeCode.UInt16);
                ReadWriteUtil.Write<UInt16>(ref dst, value);
            }
        }


        public static void Write(ref DynamicSequence dst, Int32 value)
        {
            if (0 <= value)
            {
                if (value <= VarIntTypeCode.MaxSingleValue)
                {
                    ReadWriteUtil.Write<SByte>(ref dst, (sbyte)value);
                }
                else if (value <= Int16.MaxValue)
                {
                    WriteTypeCode(ref dst, VarIntTypeCode.Int16);
                    ReadWriteUtil.Write<Int16>(ref dst, (Int16)value);
                }
                else
                {
                    WriteTypeCode(ref dst, VarIntTypeCode.Int32);
                    ReadWriteUtil.Write<Int32>(ref dst, (Int32)value);
                }
            }
            else
            {
                if (VarIntTypeCode.MinSingleValue <= value)
                {
                    ReadWriteUtil.Write<SByte>(ref dst, (sbyte)value);
                }
                else if (sbyte.MinValue <= value)
                {
                    WriteTypeCode(ref dst, VarIntTypeCode.SByte);
                    ReadWriteUtil.Write<SByte>(ref dst, (SByte)value);
                }
                else if (short.MinValue <= value)
                {
                    WriteTypeCode(ref dst, VarIntTypeCode.Int16);
                    ReadWriteUtil.Write<Int16>(ref dst, (Int16)value);
                }
                else
                {
                    WriteTypeCode(ref dst, VarIntTypeCode.Int32);
                    ReadWriteUtil.Write<Int32>(ref dst, (Int32)value);
                }
            }
        }


        public static void Write(ref DynamicSequence dst, UInt32 value)
        {
            if (value <= VarIntTypeCode.MaxSingleValue)
            {
                ReadWriteUtil.Write<SByte>(ref dst, (sbyte)value);
            }
            else if (value <= UInt16.MaxValue)
            {
                WriteTypeCode(ref dst, VarIntTypeCode.UInt16);
                ReadWriteUtil.Write<UInt16>(ref dst, (UInt16)value);
            }
            else
            {
                WriteTypeCode(ref dst, VarIntTypeCode.UInt32);
                ReadWriteUtil.Write<UInt32>(ref dst, value);
            }
        }


        public static void Write(ref DynamicSequence dst, Int64 value)
        {
            if (0 <= value)
            {
                if (value <= VarIntTypeCode.MaxSingleValue)
                {
                    ReadWriteUtil.Write<SByte>(ref dst, (sbyte)value);
                }
                else if (value <= Int16.MaxValue)
                {
                    WriteTypeCode(ref dst, VarIntTypeCode.Int16);
                    ReadWriteUtil.Write<Int16>(ref dst, (Int16)value);
                }
                else if (value <= Int32.MaxValue)
                {
                    WriteTypeCode(ref dst, VarIntTypeCode.Int32);
                    ReadWriteUtil.Write<Int32>(ref dst, (Int32)value);
                }
                else
                {
                    WriteTypeCode(ref dst, VarIntTypeCode.Int64);
                    ReadWriteUtil.Write<Int64>(ref dst, value);
                }
            }
            else
            {
                if (VarIntTypeCode.MinSingleValue <= value)
                {
                    ReadWriteUtil.Write<SByte>(ref dst, (sbyte)value);
                }
                else if (sbyte.MinValue <= value)
                {
                    WriteTypeCode(ref dst, VarIntTypeCode.SByte);
                    ReadWriteUtil.Write<SByte>(ref dst, (SByte)value);
                }
                else if (short.MinValue <= value)
                {
                    WriteTypeCode(ref dst, VarIntTypeCode.Int16);
                    ReadWriteUtil.Write<Int16>(ref dst, (Int16)value);
                }
                else if (int.MinValue <= value)
                {
                    WriteTypeCode(ref dst, VarIntTypeCode.Int32);
                    ReadWriteUtil.Write<Int32>(ref dst, (Int32)value);
                }
                else
                {
                    WriteTypeCode(ref dst, VarIntTypeCode.Int64);
                    ReadWriteUtil.Write<Int64>(ref dst, value);
                }
            }
        }

        public static void Write(ref DynamicSequence dst, UInt64 value)
        {
            if (value <= VarIntTypeCode.MaxSingleValue)
            {
                ReadWriteUtil.Write<SByte>(ref dst, (sbyte)value);
            }
            else if (value <= UInt16.MaxValue)
            {
                WriteTypeCode(ref dst, VarIntTypeCode.UInt16);
                ReadWriteUtil.Write<UInt16>(ref dst, (UInt16)value);
            }
            else if (value <= UInt32.MaxValue)
            {
                WriteTypeCode(ref dst, VarIntTypeCode.UInt32);
                ReadWriteUtil.Write<UInt32>(ref dst, (UInt32)value);
            }
            else
            {
                WriteTypeCode(ref dst, VarIntTypeCode.UInt64);
                ReadWriteUtil.Write<UInt64>(ref dst, value);
            }
        }
        public static Byte ReadByte(ref DynamicSequence src)
        {
            sbyte typeCode = ReadWriteUtil.Read<SByte>(ref src);

            return typeCode switch
            {
                VarIntTypeCode.Byte => ReadWriteUtil.Read<Byte>(ref src),
                VarIntTypeCode.SByte => checked((Byte)ReadWriteUtil.Read<SByte>(ref src)),
                VarIntTypeCode.UInt16 => checked((Byte)ReadWriteUtil.Read<UInt16>(ref src)),
                VarIntTypeCode.UInt32 => checked((Byte)ReadWriteUtil.Read<UInt32>(ref src)),
                VarIntTypeCode.UInt64 => checked((Byte)ReadWriteUtil.Read<UInt64>(ref src)),
                VarIntTypeCode.Int16 => checked((Byte)ReadWriteUtil.Read<Int16>(ref src)),
                VarIntTypeCode.Int32 => checked((Byte)ReadWriteUtil.Read<Int32>(ref src)),
                VarIntTypeCode.Int64 => checked((Byte)ReadWriteUtil.Read<Int64>(ref src)),
                _ => checked((Byte)typeCode),
            };
        }

        public static SByte ReadSByte(ref DynamicSequence src)
        {
            sbyte typeCode = ReadWriteUtil.Read<SByte>(ref src);

            return typeCode switch
            {
                VarIntTypeCode.Byte => checked((SByte)ReadWriteUtil.Read<Byte>(ref src)),
                VarIntTypeCode.SByte => ReadWriteUtil.Read<SByte>(ref src),
                VarIntTypeCode.UInt16 => checked((SByte)ReadWriteUtil.Read<UInt16>(ref src)),
                VarIntTypeCode.UInt32 => checked((SByte)ReadWriteUtil.Read<UInt32>(ref src)),
                VarIntTypeCode.UInt64 => checked((SByte)ReadWriteUtil.Read<UInt64>(ref src)),
                VarIntTypeCode.Int16 => checked((SByte)ReadWriteUtil.Read<Int16>(ref src)),
                VarIntTypeCode.Int32 => checked((SByte)ReadWriteUtil.Read<Int32>(ref src)),
                VarIntTypeCode.Int64 => checked((SByte)ReadWriteUtil.Read<Int64>(ref src)),
                _ => checked((SByte)typeCode),
            };
        }

        public static UInt16 ReadUInt16(ref DynamicSequence src)
        {
            sbyte typeCode = ReadWriteUtil.Read<SByte>(ref src);

            return typeCode switch
            {
                VarIntTypeCode.Byte => ReadWriteUtil.Read<Byte>(ref src),
                VarIntTypeCode.SByte => checked((UInt16)ReadWriteUtil.Read<SByte>(ref src)),
                VarIntTypeCode.UInt16 => ReadWriteUtil.Read<UInt16>(ref src),
                VarIntTypeCode.Int16 => checked((UInt16)ReadWriteUtil.Read<Int16>(ref src)),
                VarIntTypeCode.UInt32 => checked((UInt16)ReadWriteUtil.Read<UInt32>(ref src)),
                VarIntTypeCode.Int32 => checked((UInt16)ReadWriteUtil.Read<Int32>(ref src)),
                VarIntTypeCode.UInt64 => checked((UInt16)ReadWriteUtil.Read<UInt64>(ref src)),
                VarIntTypeCode.Int64 => checked((UInt16)ReadWriteUtil.Read<Int64>(ref src)),
                _ => checked((UInt16)typeCode),
            };
        }

        public static UInt32 ReadUInt32(ref DynamicSequence src)
        {
            sbyte typeCode = ReadWriteUtil.Read<SByte>(ref src);

            return typeCode switch
            {
                VarIntTypeCode.Byte => ReadWriteUtil.Read<Byte>(ref src),
                VarIntTypeCode.SByte => checked((UInt32)ReadWriteUtil.Read<SByte>(ref src)),
                VarIntTypeCode.UInt16 => checked((UInt32)ReadWriteUtil.Read<UInt16>(ref src)),
                VarIntTypeCode.Int16 => checked((UInt32)ReadWriteUtil.Read<Int16>(ref src)),
                VarIntTypeCode.UInt32 => ReadWriteUtil.Read<UInt32>(ref src),
                VarIntTypeCode.Int32 => checked((UInt32)ReadWriteUtil.Read<Int32>(ref src)),
                VarIntTypeCode.UInt64 => checked((UInt32)ReadWriteUtil.Read<UInt64>(ref src)),
                VarIntTypeCode.Int64 => checked((UInt32)ReadWriteUtil.Read<Int64>(ref src)),
                _ => checked((UInt32)typeCode),
            };
        }

        public static Int16 ReadInt16(ref DynamicSequence src)
        {
            sbyte typeCode = ReadWriteUtil.Read<SByte>(ref src);

            return typeCode switch
            {
                VarIntTypeCode.Byte => (Int16)ReadWriteUtil.Read<Byte>(ref src),
                VarIntTypeCode.SByte => (Int16)ReadWriteUtil.Read<SByte>(ref src),
                VarIntTypeCode.Int16 => ReadWriteUtil.Read<Int16>(ref src),
                VarIntTypeCode.Int32 => checked((Int16)ReadWriteUtil.Read<Int32>(ref src)),
                VarIntTypeCode.Int64 => checked((Int16)ReadWriteUtil.Read<Int64>(ref src)),
                _ => checked((Int16)typeCode),
            };
        }

        public static Int32 ReadInt32(ref DynamicSequence src)
        {
            sbyte typeCode = ReadWriteUtil.Read<SByte>(ref src);

            return typeCode switch
            {
                VarIntTypeCode.Byte => (Int32)ReadWriteUtil.Read<Byte>(ref src),
                VarIntTypeCode.SByte => (Int32)ReadWriteUtil.Read<SByte>(ref src),
                VarIntTypeCode.UInt16 => (Int32)ReadWriteUtil.Read<UInt16>(ref src),
                VarIntTypeCode.Int16 => (Int32)ReadWriteUtil.Read<Int16>(ref src),
                VarIntTypeCode.UInt32 => checked((Int32)ReadWriteUtil.Read<UInt32>(ref src)),
                VarIntTypeCode.Int32 => ReadWriteUtil.Read<Int32>(ref src),
                VarIntTypeCode.UInt64 => checked((Int32)ReadWriteUtil.Read<UInt64>(ref src)),
                VarIntTypeCode.Int64 => checked((Int32)ReadWriteUtil.Read<Int64>(ref src)),
                _ => checked((Int32)typeCode),
            };
        }

        public static Int64 ReadInt64(ref DynamicSequence src)
        {
            sbyte typeCode = ReadWriteUtil.Read<SByte>(ref src);

            return typeCode switch
            {
                VarIntTypeCode.Byte => (Int64)ReadWriteUtil.Read<Byte>(ref src),
                VarIntTypeCode.SByte => (Int64)ReadWriteUtil.Read<SByte>(ref src),
                VarIntTypeCode.UInt16 => (Int64)ReadWriteUtil.Read<UInt16>(ref src),
                VarIntTypeCode.Int16 => (Int64)ReadWriteUtil.Read<Int16>(ref src),
                VarIntTypeCode.UInt32 => (Int64)ReadWriteUtil.Read<UInt32>(ref src),
                VarIntTypeCode.Int32 => (Int64)ReadWriteUtil.Read<Int32>(ref src),
                VarIntTypeCode.UInt64 => checked((Int64)ReadWriteUtil.Read<UInt64>(ref src)),
                VarIntTypeCode.Int64 => ReadWriteUtil.Read<Int64>(ref src),
                _ => checked((Int64)typeCode),
            };
        }

        public static UInt64 ReadUInt64(ref DynamicSequence src)
        {
            sbyte typeCode = ReadWriteUtil.Read<SByte>(ref src);

            return typeCode switch
            {
                VarIntTypeCode.Byte => (UInt64)ReadWriteUtil.Read<Byte>(ref src),
                VarIntTypeCode.SByte => checked((UInt64)ReadWriteUtil.Read<SByte>(ref src)),
                VarIntTypeCode.UInt16 => (UInt64)ReadWriteUtil.Read<UInt16>(ref src),
                VarIntTypeCode.Int16 => checked((UInt64)ReadWriteUtil.Read<Int16>(ref src)),
                VarIntTypeCode.UInt32 => (UInt64)ReadWriteUtil.Read<UInt32>(ref src),
                VarIntTypeCode.Int32 => checked((UInt64)ReadWriteUtil.Read<Int32>(ref src)),
                VarIntTypeCode.UInt64 => ReadWriteUtil.Read<UInt64>(ref src),
                VarIntTypeCode.Int64 => checked((UInt64)ReadWriteUtil.Read<Int64>(ref src)),
                _ => checked((UInt64)typeCode),
            };
        }

    }
}
