using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyIO
{
    /// <summary>
    /// Represents a 16-bit bit field.
    /// </summary>
    public struct BitField16 : IBitField
    {
        [Endianness(Endian.Big)]
        private ushort field;

        /// <summary>
        /// Creates a new instance of the EasyIO.BitField8 class from the specified data.
        /// </summary>
        /// <param name="data">The data to create the bit field form.</param>
        public BitField16(ushort data)
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
            field = 0xFFFF;
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
            return 16 - Utils.NumberOfSetBits(field);
        }

        /// <summary>
        /// Inverts the flags in the bitfield.
        /// </summary>
        public void Invert()
        {
            field = (ushort)~field;
        }

        /// <summary>
        /// Returns the bitfield as a single unsigned byte.
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            return BitConverter.GetBytes(field);
        }
    }
}
