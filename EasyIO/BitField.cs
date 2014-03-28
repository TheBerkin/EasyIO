﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace EasyIO
{
    /// <summary>
    /// Represents a bit field of arbitrary length.
    /// </summary>
    public class BitField
    {
        private byte[] _field;

        /// <summary>
        /// Creates a new instance of the EasyIO.BitField class with the specified number of bits.
        /// </summary>
        /// <param name="bits">The number of bits in the bit field. This value must be a multiple of 8.</param>
        public BitField(int bits)
        {
            if (bits % 8 != 0)
            {
                throw new ArgumentException("Bit count must be a multiple of 8.");
            }

            _field = new byte[bits / 8];
        }

        internal BitField(byte[] data)
        {
            _field = data;
        }

        /// <summary>
        /// Creates a BitField object from the specified data.
        /// </summary>
        /// <typeparam name="T">The type of data to pass.</typeparam>
        /// <param name="value">The data to pass to the BitField.</param>
        /// <returns></returns>
        public static BitField FromValue<T>(T value) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            byte[] data = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(value, ptr, false);
            Marshal.Copy(ptr, data, 0, size);
            return new BitField(data);
        }

        /// <summary>
        /// Accesses the bit at the specified index in the bit field.
        /// </summary>
        /// <param name="i">The index of the bit to access.</param>
        /// <returns></returns>
        public bool this[int i]
        {
            get 
            {
                return ((_field[i / 8] >> (i % 8)) & 1) == 1;
            }
            set
            {
                    _field[i / 8] = (byte)((_field[i / 8] & ~(1 << (i % 8))) | ((value ? 1 << i % 8 : 0)));
            }
        }

        /// <summary>
        /// The number of bits in the BitField.
        /// </summary>
        public int Bits
        {
            get { return _field.Length * 8; }
        }

        /// <summary>
        /// The number of bytes in the BitField.
        /// </summary>
        public int Bytes
        {
            get { return _field.Length; }
        }

        /// <summary>
        /// Unsets all the flags in the bitfield.
        /// </summary>
        public void UnsetAll()
        {
            Array.Clear(_field, 0, _field.Length);
        }

        /// <summary>
        /// Sets all the flags in the bitfield.
        /// </summary>
        public void SetAll()
        {
            for(int i = 0; i < _field.Length; i++)
            {
                _field[i] = 0xFF;
            }
        }

        /// <summary>
        /// Returns the number of set bits.
        /// </summary>
        /// <returns>The number of set bits.</returns>
        public int GetSetCount()
        {
            int n = 0;
            foreach(byte b in _field)
            {
                n += Utils.NumberOfSetBits(b);
            }
            return n;
        }

        /// <summary>
        /// Returns the number of unset bits.
        /// </summary>
        /// <returns>The number of unset bits.</returns>
        public int GetUnsetCount()
        {
            return this.Bits - GetSetCount();
        }

        /// <summary>
        /// Inverts the flags in the bit field.
        /// </summary>
        public void Invert()
        {
            for(int i = 0; i < _field.Length; i++)
            {
                _field[i] = (byte)~_field[i];
            }
        }

        /// <summary>
        /// Creates a new BitField from the specified array of bytes.
        /// </summary>
        /// <param name="data">The array of bytes to create the BitField from.</param>
        /// <returns></returns>
        public static BitField FromBytes(byte[] data)
        {
            var bf = new BitField(data.Length * 8);
            Array.Copy(data, bf._field, data.Length);
            return bf;
        }

        /// <summary>
        /// Returns the BitField as an array of bytes.
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            return _field;
        }
    }
}
