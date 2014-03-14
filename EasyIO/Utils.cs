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

        public static void ConvertStructEndians<TStruct>(ref TStruct o) where TStruct : struct
        {
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
                            object value = field.GetValue(o);

                            // Transfer the field value to the pointer and copy it to the array.
                            Marshal.StructureToPtr(value, vptr, false);
                            Marshal.Copy(vptr, vData, 0, length);

                            // Reverse that shit.
                            Array.Reverse(vData);

                            // Copy it back to the pointer.
                            Marshal.Copy(vData, 0, vptr, length);

                            // Plug it back into the field.
                            field.SetValue(o, Marshal.PtrToStructure(vptr, ftype));

                            // Deallocate the pointer.
                            Marshal.FreeHGlobal(vptr);
                        }
                        break; // Go to the next field.
                    }
                }
            }
        }
    }
}