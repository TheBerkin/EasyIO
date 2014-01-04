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
    public class Bitfield8
    {
        private bool[] bits;
        private const int N = 8;
        
        public Bitfield8()
        {
            bits = new bool[N];
        }

        public Bitfield8(byte data)
        {
            bits = new bool[N];

            for(int i = 0; i < N; i++)
            {
                bits[i] = data.GetFlag(i);
            }
        }

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
        public static Bitfield8 FromStream(Stream stream)
        {
            int b = stream.ReadByte();
            if (b < 0)
            {
                throw new EndOfStreamException();
            }

            var bf = new Bitfield8();
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

        public static implicit operator byte(Bitfield8 bitfield)
        {
            return bitfield.GetByte();
        }

        public static implicit operator Bitfield8(byte data)
        {
            return new Bitfield8(data);
        }

        public static bool operator ==(Bitfield8 a, Bitfield8 b)
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

        public static bool operator !=(Bitfield8 a, Bitfield8 b)
        {
            return !(a == b);
        }

        public static Bitfield8 operator &(Bitfield8 a, Bitfield8 b)
        {
            Bitfield8 bf = new Bitfield8();
            for(int i = 0; i < N; i++)
            {
                bf.bits[i] = a.bits[i] && b.bits[i];
            }
            return bf;
        }

        public static Bitfield8 operator |(Bitfield8 a, Bitfield8 b)
        {
            Bitfield8 bf = new Bitfield8();
            for (int i = 0; i < N; i++)
            {
                bf.bits[i] = a.bits[i] || b.bits[i];
            }
            return bf;
        }

        public static Bitfield8 operator !(Bitfield8 a)
        {
            var bf = new Bitfield8(a.GetByte());
            bf.Invert();
            return bf;
        }

        public override bool Equals(object obj)
        {
            var bf = obj as Bitfield8;

            if ((object)bf == null)
            {
                return false;
            }

            return this == bf;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ GetByte();
        }
    }
}
