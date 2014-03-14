using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

namespace EasyIO
{
    /// <summary>
    /// Provides comprehensive binary data reading functionality including support for reading arrays and enumeration members.
    /// </summary>
    public class EasyReader : IDisposable
    {
        private Stream _stream;
        private Endianness _defEndian;

        /// <summary>
        /// Creates a new instance of the EasyIO.EasyReader class from the specified stream.
        /// </summary>
        /// <param name="stream">The stream from which to read.</param>
        /// <param name="startIndex">The index at which to start reading.</param>
        /// <param name="defaultEndianness">The endianness of the data to be read.</param>
        public EasyReader(Stream stream, int startIndex = 0, Endianness defaultEndianness = Endianness.Little)
        {
            _stream = stream;
            _stream.Position = startIndex;
            _defEndian = defaultEndianness;
        }

        /// <summary>
        /// Creates a new instance of the EasyIO.EasyReader class from the specified file path.
        /// </summary>
        /// <param name="path">The path to the file to read.</param>
        /// <param name="mode">Speficies how the operating system should open the file.</param>
        /// <param name="startIndex">The index at which to start reading.</param>
        /// <param name="defaultEndianness">The endianness of the data to be read.</param>
        public EasyReader(string path, FileMode mode, int startIndex = 0, Endianness defaultEndianness = Endianness.Little)
        {
            _stream = File.Open(path, mode);
            _stream.Position = startIndex;
            _defEndian = defaultEndianness;
        }

        /// <summary>
        /// Creates a new instance of the EasyIO.EasyReader class from a byte array.
        /// </summary>
        /// <param name="data">The byte array to read from.</param>
        /// <param name="startIndex">The index at which to start reading.</param>
        /// <param name="defaultEndianness">The endianness of the data to be read.</param>
        public EasyReader(byte[] data, int startIndex = 0, Endianness defaultEndianness = Endianness.Little)
        {
            _stream = new MemoryStream(data);
            _stream.Position = startIndex;
            _defEndian = defaultEndianness;
        }

        /// <summary>
        /// The endianness in which data is read by the stream.
        /// </summary>
        public Endianness Endianness
        {
            get { return _defEndian; }
        }

        /// <summary>
        /// Returns true if the stream has reached its end.
        /// </summary>
        public bool EndOfStream
        {
            get { return _stream.Position == _stream.Length; }
        }

        /// <summary>
        /// The amount of bytes that are remaining to be read.
        /// </summary>
        public long Remaining
        {
            get { return _stream.Length - _stream.Position; }
        }

        /// <summary>
        /// The length of the stream in bytes.
        /// </summary>
        public long Length
        {
            get { return _stream.Length; }
        }

        /// <summary>
        /// The underlying stream for this instance.
        /// </summary>
        public Stream BaseStream
        {
            get { return _stream; }
        }

        private static void ConvertToSystemEndian(byte[] data, Endianness dataEndianness)
        {
            if (BitConverter.IsLittleEndian)
            {
                if (dataEndianness == EasyIO.Endianness.Big)
                {                    
                    Array.Reverse(data);
                }
            }
            else if (dataEndianness == EasyIO.Endianness.Little)
            {
                Array.Reverse(data);
            }
        }

        private static bool IsNumericType(Type t)
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

        private byte[] ReadAndFormat(int count)
        {
            byte[] buffer = new byte[count];
            _stream.Read(buffer, 0, count);
            ConvertToSystemEndian(buffer, _defEndian);
            return buffer;
        }

        /// <summary>
        /// Returns the next available byte but does not consume it.
        /// </summary>
        /// <returns></returns>
        public int Peek()
        {
            int c = _stream.ReadByte();
            _stream.Position--;
            return c;
        }

        /// <summary>
        /// Reads a single byte.
        /// </summary>
        /// <returns></returns>
        public byte ReadByte()
        {
            return (byte)_stream.ReadByte();
        }

        /// <summary>
        /// Reads an array of bytes.
        /// </summary>
        /// <param name="count">The number of bytes to read.</param>
        /// <returns></returns>
        public byte[] ReadBytes(int count)
        {
            byte[] buffer = new byte[count];
            _stream.Read(buffer, 0, count);
            return buffer;
        }

        /// <summary>
        /// Reads all bytes from the stream.
        /// </summary>
        /// <returns></returns>
        public byte[] ReadAllBytes()
        {
            byte[] buffer = new byte[_stream.Length];
            _stream.Read(buffer, 0, buffer.Length);
            return buffer;
        }

        /// <summary>
        /// Reads a signed byte.
        /// </summary>
        /// <returns></returns>
        public sbyte ReadSByte()
        {
            return (sbyte)_stream.ReadByte();
        }

        /// <summary>
        /// Reads a Unicode character.
        /// </summary>
        /// <returns></returns>
        public char ReadChar()
        {
            return BitConverter.ToChar(ReadAndFormat(2), 0);
        }

        /// <summary>
        /// Reads a 1-byte boolean value.
        /// </summary>
        /// <returns></returns>
        public bool ReadBoolean()
        {
            return ReadByte() != 0;
        }

        /// <summary>
        /// Reads a series of Unicode characters.
        /// </summary>
        /// <param name="count">The number of characters to read.</param>
        /// <returns></returns>
        public char[] ReadChars(int count)
        {
            return Encoding.Unicode.GetChars(ReadBytes(count));
        }

        /// <summary>
        /// Reads a 16-bit unsigned integer.
        /// </summary>
        /// <returns></returns>
        public ushort ReadUInt16()
        {
            return BitConverter.ToUInt16(ReadAndFormat(2), 0);
        }

        /// <summary>
        /// Reads a 16-bit signed integer.
        /// </summary>
        /// <returns></returns>
        public short ReadInt16()
        {
            return BitConverter.ToInt16(ReadAndFormat(2), 0);
        }

        /// <summary>
        /// Reads a 32-bit unsigned integer.
        /// </summary>
        /// <returns></returns>
        public uint ReadUInt32()
        {
            return BitConverter.ToUInt32(ReadAndFormat(4), 0);
        }

        /// <summary>
        /// Reads a 32-bit signed integer.
        /// </summary>
        /// <returns></returns>
        public int ReadInt32()
        {
            return BitConverter.ToInt32(ReadAndFormat(4), 0);
        }

        /// <summary>
        /// Reads a 64-bit unsigned integer.
        /// </summary>
        /// <returns></returns>
        public ulong ReadUInt64()
        {
            return BitConverter.ToUInt64(ReadAndFormat(8), 0);
        }

        /// <summary>
        /// Reads a 64-bit signed integer.
        /// </summary>
        /// <returns></returns>
        public long ReadInt64()
        {
            return BitConverter.ToInt64(ReadAndFormat(8), 0);
        }

        /// <summary>
        /// Reads a single-precision floating point number.
        /// </summary>
        /// <returns></returns>
        public float ReadSingle()
        {
            return BitConverter.ToSingle(ReadAndFormat(4), 0);
        }

        /// <summary>
        /// Reads a double-precision floating-point number.
        /// </summary>
        /// <returns></returns>
        public double ReadDouble()
        {
            return BitConverter.ToDouble(ReadAndFormat(8), 0);
        }

        /// <summary>
        /// Reads a 128-bit decimal number.
        /// </summary>
        /// <returns></returns>
        public decimal ReadDecimal()
        {
            return ReadStruct<decimal>();
        }

        /// <summary>
        /// Reads a Unicode string.
        /// </summary>
        /// <returns></returns>
        public string ReadString()
        {
            int bytes = ReadInt32();
            return Encoding.Unicode.GetString(ReadBytes(bytes));
        }

        /// <summary>
        /// Reads an 8-bit bit field.
        /// </summary>
        /// <returns></returns>
        public BitField8 ReadBitfield8()
        {
            return BitField8.FromStream(_stream);
        }

        /// <summary>
        /// Reads a string encoded in the specified encoding.
        /// </summary>
        /// <param name="encoding">The encoding of the string to be read.</param>
        /// <returns></returns>
        public string ReadString(Encoding encoding)
        {
            int bytes = ReadInt32();
            return encoding.GetString(ReadBytes(bytes));
        }

        /// <summary>
        /// Reads an array of the specified type.
        /// </summary>
        /// <typeparam name="T">The type stored in the array.</typeparam>
        /// <param name="use64bit">Indicates to the reader that the array length is 64-bit rather than 32-bit.</param>
        /// <returns></returns>
        public T[] ReadArray<T>(bool use64bit = false) where T : struct
        {
            bool isNumeric = IsNumericType(typeof(T));
            long count = use64bit ? ReadInt64() : ReadInt32();            
            T[] array = new T[count];
            for(int i = 0; i < count; i++)
            {
                array[i] = ReadStruct<T>(isNumeric);
            }
            return array;
        }

        /// <summary>
        /// Reads a dictionary of the specified key and value types.
        /// </summary>
        /// <typeparam name="K">The key type of the dictionary.</typeparam>
        /// <typeparam name="V">The value type of the dictionary.</typeparam>
        /// <param name="use64bit">Indicates to the reader that the dictionary length is 64-bit rather than 32-bit.</param>
        /// <returns></returns>
        public Dictionary<K, V> ReadDictionary<K, V>() 
            where K : struct
            where V : struct
        {
            bool isKNumeric = IsNumericType(typeof(K));
            bool isVNumeric = IsNumericType(typeof(V));
            int count = ReadInt32();
            var dict = new Dictionary<K, V>(count);
            K key;
            V value;
            for(int i = 0; i < count; i++)
            {
                key = ReadStruct<K>(isKNumeric);
                value = ReadStruct<V>(isVNumeric);
                dict.Add(key, value);
            }
            return dict;
        }

        /// <summary>
        /// Reads an enumeration member.
        /// </summary>
        /// <typeparam name="T">The enumeration type to read.</typeparam>
        /// <returns></returns>
        public T ReadEnum<T>() where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type.");
            }
            byte size = (byte)Marshal.SizeOf(Enum.GetUnderlyingType(typeof(T)));
            byte[] data = ReadAndFormat(size);
            return (T)Enum.ToObject(typeof(T), BitConverter.ToInt64(data, 0));
        }

        /// <summary>
        /// Reads a struct of the specified type.
        /// </summary>
        /// <typeparam name="T">The struct to read.</typeparam>
        /// <returns></returns>
        public T ReadStruct<T>(bool convertEndian = false) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            byte[] data = convertEndian ? ReadAndFormat(size) : ReadBytes(size);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(data, 0, ptr, size);
            T i = (T)Marshal.PtrToStructure(ptr, typeof(T));
            Marshal.FreeHGlobal(ptr);
            return i;
        }

        /// <summary>
        /// Closes the reader and the underlying stream.
        /// </summary>
        public void Close()
        {
            _stream.Close();
        }

        /// <summary>
        /// Releases all resources used by the current instance of the EasyIO.EasyReader class.
        /// </summary>
        void IDisposable.Dispose()
        {
            _stream.Dispose();
        }
    }
}
