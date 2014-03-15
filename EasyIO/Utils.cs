﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.InteropServices;

namespace EasyIO
{
    internal static class Utils
    {
        public static int NumberOfSetBits(uint i)
        {
            i = i - ((i >> 1) & 0x55555555);
            i = (i & 0x33333333) + ((i >> 2) & 0x33333333);
            return (int)(((i + (i >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
        }

        public static bool EndianConvertNeeded(Endian endianness)
        {
            return (BitConverter.IsLittleEndian && endianness == Endian.Big) || (!BitConverter.IsLittleEndian && endianness == Endian.Little);
        }

        public static bool IsNumericType(Type t)
        {
            switch (Type.GetTypeCode(t))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Converts the endianness of a series of bytes according to the endianness of the data. This process works both for system-side and data-side conversions.
        /// </summary>
        /// <param name="data">The data to convert.</param>
        /// <param name="dataEndianness">The endianness to convert to or from.</param>
        public static void ConvertEndian(byte[] data, Endian dataEndianness)
        {
            if (BitConverter.IsLittleEndian)
            {
                if (dataEndianness == EasyIO.Endian.Big)
                {
                    Array.Reverse(data);
                }
            }
            else if (dataEndianness == EasyIO.Endian.Little)
            {
                Array.Reverse(data);
            }
        }

        public static void ConvertStructEndians<TStruct>(ref TStruct o)
        {
            if (!typeof(TStruct).IsValueType)
            {
                throw new ArgumentException("TStruct must be a value type.");
            }
            object boxed = o;
            foreach (var field in typeof(TStruct).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                Type ftype = field.FieldType;
                if (!Utils.IsNumericType(ftype))
                {
                    continue;
                }

                var attrs = field.GetCustomAttributes(true);
                foreach (var attr in attrs)
                {
                    if (attr.GetType() == typeof(EndiannessAttribute))
                    {
                        var endian = ((EndiannessAttribute)attr).Endianness;
                        if (Utils.EndianConvertNeeded(endian))
                        {
                            // Get the field size, allocate a pointer and a buffer for flipping bytes.
                            int length = Marshal.SizeOf(ftype);
                            IntPtr vptr = Marshal.AllocHGlobal(length);
                            byte[] vData = new byte[length];

                            // Fetch the field value and store it.
                            object value = field.GetValue(boxed);

                            // Transfer the field value to the pointer and copy it to the array.
                            Marshal.StructureToPtr(value, vptr, false);
                            Marshal.Copy(vptr, vData, 0, length);
                            
                            // Reverse that shit.
                            Array.Reverse(vData);

                            // Copy it back to the pointer.
                            Marshal.Copy(vData, 0, vptr, length);
                            value = Marshal.PtrToStructure(vptr, ftype);
                            // Plug it back into the field.
                            field.SetValue(boxed, value);
                            // Deallocate the pointer.
                            Marshal.FreeHGlobal(vptr);
                            o = (TStruct)boxed;
                        }
                        break; // Go to the next field.
                    }
                }
            }
        }
    }
}
