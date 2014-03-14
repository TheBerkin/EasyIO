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
    public class BitField8
    {
        private bool[] bits;
        private const int N = 8;
        
        /// <summary>
        /// Creates a new instance of the EasyIO.BitField8 class.
        /// </summary>
        public BitField8()
        {
            bits = new bool[N];
        }

        /// <summary>
        /// Creates a new instance of the EasyIO.BitField8 class from the specified data.
        /// </summary>
        /// <param name="data">The data to create the bit field form.</param>
        public BitField8(byte data)
        {
            bits = new bool[N];

            for(int i = 0; i < N; i++)
            {
                bits[i] = data.GetFlag(i);
            }
        }

        /// <summary>
        /// Accesses the bit at the specified index in the bit field.
        /// </summary>
        /// <param name="i">The index of the bit to access.</param>
        /// <returns></returns>
        public bool this[int i]
        {
            get { return bits[i]; }
            set { bits[i] = value; }
        }

        /// <summary>
        /// Unsets all the flags in the bitfield.
        /// </summary>
        public void UnsetAll()
        {
            Array.Clear(bits, 0, N);
        }

        /// <summary>
        /// Sets all the flags in the bitfield.
        /// </summary>
        public void SetAll()
        {
            for(int i = 0; i < N; i++)
            {
                bits[i] = true;
            }
        }

        /// <summary>
        /// Returns the number of set flags.
        /// </summary>
        /// <returns>The number of set flags.</returns>
        public int GetSetCount()
        {
            int c = 0;
            for (int i = 0; i < N; i++)
            {
                if (bits[i])
                {
                    c++;
                }
            }
            return c;
        }

        /// <summary>
        /// Returns the number of unset flags.
        /// </summary>
        /// <returns>The number of unset flags.</returns>
        public int GetUnsetCount()
        {
            int c = 0;
            for (int i = 0; i < N; i++)
            {
                if (!bits[i])
                {
                    c++;
                }
            }
            return c;
        }

        /// <summary>
        /// Inverts the flags in the bitfield.
        /// </summary>
        public void Invert()
        {
            for(int i = 0; i < N; i++)
            {
                bits[i] = !bits[i];
            }
        }

        /// <summary>
        /// Retrieves a bitfield from the specified stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>The read bitfield.</returns>
        public static BitField8 FromStream(Stream stream)
        {
            int b = stream.ReadByte();
            if (b < 0)
            {
                throw new EndOfStreamException();
            }

            var bf = new BitField8();
            for(int i = 0; i < N; i++)
            {
                bf.bits[i] = b.GetFlag(i);
            }

            return bf;
        }

        /// <summary>
        /// Writes the bitfield to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(this.GetByte());
        }

        /// <summary>
        /// Returns the bitfield as a single unsigned byte.
        /// </summary>
        /// <returns></returns>
        public byte GetByte()
        {
            int b = 0;
            for(int i = 0; i < N; i++)
            {
                b |= (0x1 << i);
            }
            return (byte)b;
        }

        /// <summary>
        /// Converts a BitField8 to a byte.
        /// </summary>
        /// <param name="bitfield">The bit field to convert.</param>
        /// <returns></returns>
        public static implicit operator byte(BitField8 bitfield)
        {
            return bitfield.GetByte();
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
        /// Tests equality between two bit fields.
        /// </summary>
        /// <param name="a">The first bit field.</param>
        /// <param name="b">The second bit field.</param>
        /// <returns></returns>
        public static bool operator ==(BitField8 a, BitField8 b)
        {
            for(int i = 0; i < N; i++)
            {
                if (!a.bits[i] && b.bits[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Tests inequality between two bit fields.
        /// </summary>
        /// <param name="a">The first bit field.</param>
        /// <param name="b">The second bit field.</param>
        /// <returns></returns>
        public static bool operator !=(BitField8 a, BitField8 b)
        {
            return !(a == b);
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
            for(int i = 0; i < N; i++)
            {
                bf.bits[i] = a.bits[i] && b.bits[i];
            }
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
            for (int i = 0; i < N; i++)
            {
                bf.bits[i] = a.bits[i] || b.bits[i];
            }
            return bf;
        }

        /// <summary>
        /// Performs an NOT operation on two bit fields.
        /// </summary>
        /// <param name="a">The first bit field.</param>
        /// <returns></returns>
        public static BitField8 operator ~(BitField8 a)
        {
            var bf = new BitField8(a.GetByte());
            bf.Invert();
            return bf;
        }
        
        public override bool Equals(object obj)
        {
            var bf = obj as BitField8;

            if ((object)bf == null)
            {
                return false;
            }

            return this == bf;
        }

        /// <summary>
        /// Retrieves a hash code that nobody actually uses.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode() ^ GetByte();
        }
    }
}
