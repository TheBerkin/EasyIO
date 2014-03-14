using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace EasyIO
{    
    /// <summary>
    /// Represents an 8-bit bitfield.
    /// </summary>
    public struct BitField8 : IBitField
    {
        private byte field;

        /// <summary>
        /// Creates a new instance of the EasyIO.BitField8 class from the specified data.
        /// </summary>
        /// <param name="data">The data to create the bit field form.</param>
        public BitField8(byte data)
        {
            field = data;
        }

        /// <summary>
        /// Accesses the bit at the specified index in the bit field.
        /// </summary>
        /// <param name="i">The index of the bit to access.</param>
        /// <returns></returns>
        public bool this[int i]
        {
            get { return (field & (1 << i)) == 1; }
            set { field = (byte)((field & ~(1 << i)) | ((value ? 1 : 0) << i)); }
        }

        /// <summary>
        /// Unsets all the flags in the bitfield.
        /// </summary>
        public void UnsetAll()
        {
            field = 0;
        }

        /// <summary>
        /// Sets all the flags in the bitfield.
        /// </summary>
        public void SetAll()
        {
            field = 0xFF;
        }

        /// <summary>
        /// Returns the number of set flags.
        /// </summary>
        /// <returns>The number of set flags.</returns>
        public int GetSetCount()
        {
            return Utils.NumberOfSetBits(field);
        }

        /// <summary>
        /// Returns the number of unset flags.
        /// </summary>
        /// <returns>The number of unset flags.</returns>
        public int GetUnsetCount()
        {
            return 8 - Utils.NumberOfSetBits(field);
        }

        /// <summary>
        /// Inverts the flags in the bitfield.
        /// </summary>
        public void Invert()
        {
            field = (byte)~field;
        }

        /// <summary>
        /// Returns the bitfield as a single unsigned byte.
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            return new[] { field };
        }

        /// <summary>
        /// Converts a BitField8 to a byte.
        /// </summary>
        /// <param name="bitfield">The bit field to convert.</param>
        /// <returns></returns>
        public static implicit operator byte(BitField8 bitfield)
        {
            return bitfield.field;
        }

        /// <summary>
        /// Converts a byte to a BitField8.
        /// </summary>
        /// <param name="data">The byte to convert.</param>
        /// <returns></returns>
        public static implicit operator BitField8(byte data)
        {
            return new BitField8(data);
        }

        /// <summary>
        /// Performs an AND operation on two bit fields.
        /// </summary>
        /// <param name="a">The first bit field.</param>
        /// <param name="b">The second bit field.</param>
        /// <returns></returns>
        public static BitField8 operator &(BitField8 a, BitField8 b)
        {
            BitField8 bf = new BitField8();
            bf.field = (byte)(a.field & b.field);
            return bf;
        }

        /// <summary>
        /// Performs an OR operation on two bit fields.
        /// </summary>
        /// <param name="a">The first bit field.</param>
        /// <param name="b">The second bit field.</param>
        /// <returns></returns>
        public static BitField8 operator |(BitField8 a, BitField8 b)
        {
            BitField8 bf = new BitField8();
            bf.field = (byte)(a.field | b.field);
            return bf;
        }

        /// <summary>
        /// Performs an NOT operation on two bit fields.
        /// </summary>
        /// <param name="a">The first bit field.</param>
        /// <returns></returns>
        public static BitField8 operator ~(BitField8 a)
        {
            return new BitField8((byte)~a.field);
        }

        /// <summary>
        /// Gets the string representation for this instance.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("BitField8: {0:X8}", field);
        }
    }
}
