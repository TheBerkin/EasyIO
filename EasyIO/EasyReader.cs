using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace EasyIO
{
    /// <summary>
    /// Provides comprehensive binary data reading functionality including support for reading arrays and enumeration members.
    /// </summary>
    public class EasyReader : IDisposable
    {
        private Stream _stream;
        private Endian _endian;
        private bool _leaveOpen;

        /// <summary>
        /// Creates a new instance of the EasyIO.EasyReader class from the specified stream.
        /// </summary>
        /// <param name="stream">The stream from which to read.</param>
        /// <param name="startIndex">The index at which to start reading.</param>
        /// <param name="defaultEndianness">The endianness of the data to be read.</param>
        public EasyReader(Stream stream, int startIndex = 0, Endian defaultEndianness = Endian.Little)
        {
            _stream = stream;
            _stream.Position = startIndex;
            _endian = defaultEndianness;
            _leaveOpen = false;
        }

        /// <summary>
        /// Creates a new instance of the EasyIO.EasyReader class from the specified stream.
        /// </summary>
        /// <param name="stream">The stream from which to read.</param>
        /// <param name="leaveOpen">Specifies whether or not to leave the stream open after the reader is disposed.</param>
        /// <param name="startIndex">The index at which to start reading.</param>
        /// <param name="defaultEndianness">The endianness of the data to be read.</param>
        public EasyReader(Stream stream, bool leaveOpen, int startIndex = 0, Endian defaultEndianness = Endian.Little)
        {
            _stream = stream;
            _stream.Position = startIndex;
            _endian = defaultEndianness;
            _leaveOpen = leaveOpen;
        }

        /// <summary>
        /// Creates a new instance of the EasyIO.EasyReader class from the specified file path.
        /// </summary>
        /// <param name="path">The path to the file to read.</param>
        /// <param name="mode">Speficies how the operating system should open the file.</param>
        /// <param name="startIndex">The index at which to start reading.</param>
        /// <param name="defaultEndianness">The endianness of the data to be read.</param>
        public EasyReader(string path, FileMode mode = FileMode.Open, int startIndex = 0, Endian defaultEndianness = Endian.Little)
        {
            _stream = File.Open(path, mode);
            _stream.Position = startIndex;
            _endian = defaultEndianness;
            _leaveOpen = false;
        }

        /// <summary>
        /// Creates a new instance of the EasyIO.EasyReader class from a byte array.
        /// </summary>
        /// <param name="data">The byte array to read from.</param>
        /// <param name="startIndex">The index at which to start reading.</param>
        /// <param name="defaultEndianness">The endianness of the data to be read.</param>
        public EasyReader(byte[] data, int startIndex = 0, Endian defaultEndianness = Endian.Little)
        {
            _stream = new MemoryStream(data);
            _stream.Position = startIndex;
            _endian = defaultEndianness;
        }

        /// <summary>
        /// Gets or sets the endianness in which data is read by the stream.
        /// </summary>
        public Endian Endianness
        {
            get { return _endian; }
            set { _endian = value; }
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
        /// Reads an array of Unicode strings.
        /// </summary>
        /// <returns></returns>
        public string[] ReadStringArray()
        {
            int length = ReadInt32();
            string[] array = new string[length];
            for(int i = 0; i < length; i++)
            {
                array[i] = ReadString();
            }
            return array;
        }

        /// <summary>
        /// Reads a string array encoded in the specified encoding.
        /// </summary>
        /// <param name="encoding">The encoding of the strings to be read.</param>
        /// <returns></returns>
        public string[] ReadStringArray(Encoding encoding)
        {
            int length = ReadInt32();
            string[] array = new string[length];
            for(int i = 0; i < length; i++)
            {
                array[i] = ReadString(encoding);
            }
            return array;
        }

        /// <summary>
        /// Reads an array of the specified type.
        /// </summary>
        /// <typeparam name="T">The type stored in the array.</typeparam>
        /// <param name="use64bit">Indicates to the reader that the array length is 64-bit rather than 32-bit.</param>
        /// <returns></returns>
        public T[] ReadArray<T>(bool use64bit = false) where T : struct
        {
            bool isNumeric = Utils.IsNumericType(typeof(T));
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
        /// <typeparam name="TKey">The key type of the dictionary.</typeparam>
        /// <typeparam name="TValue">The value type of the dictionary.</typeparam>
        /// <returns></returns>
        public Dictionary<TKey, TValue> ReadDictionary<TKey, TValue>() 
            where TKey : IConvertible
            where TValue : IConvertible
        {
            var ktype = typeof(TKey);
            bool kIsString = ktype == typeof(String);
            var vtype = typeof(TValue);
            bool vIsString = vtype == typeof(String);

            if (!ktype.IsValueType && !kIsString)
            {
                throw new ArgumentException("TKey must be either a value type or System.String.");
            }
            else if (!vtype.IsValueType && !vIsString)
            {
                throw new ArgumentException("TVaue must be either a value type or System.String.");
            }

            bool isKNumeric = Utils.IsNumericType(typeof(TKey));
            bool isVNumeric = Utils.IsNumericType(typeof(TValue));
            int count = ReadInt32();
            var dict = new Dictionary<TKey, TValue>(count);
            TKey key;
            TValue value;
            for(int i = 0; i < count; i++)
            {
                if (kIsString)
                {
                    key = (TKey)((object)ReadString());
                }
                else
                {
                    key = ReadStruct<TKey>(isKNumeric);
                }
                
                if (vIsString)
                {
                    value = (TValue)((object)ReadString());
                }
                else
                {
                    value = ReadStruct<TValue>(isVNumeric);
                }

                dict.Add(key, value);
            }
            return dict;
        }

        /// <summary>
        /// Reads an enumeration member.
        /// </summary>
        /// <typeparam name="TEnum">The enumeration type to read.</typeparam>
        /// <returns></returns>
        public TEnum ReadEnum<TEnum>() where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type.");
            }
            byte size = (byte)Marshal.SizeOf(Enum.GetUnderlyingType(typeof(TEnum)));
            byte[] data = ReadAndFormat(size);
            Array.Resize(ref data, 8);
            return (TEnum)Enum.ToObject(typeof(TEnum), BitConverter.ToInt64(data, 0));
        }

        /// <summary>
        /// Reads a struct of the specified type.
        /// </summary>
        /// <typeparam name="TStruct">The struct to read.</typeparam>
        /// <returns></returns>
        public TStruct ReadStruct<TStruct>(bool convertEndian = true)
        {
            if (!typeof(TStruct).IsValueType)
            {
                throw new ArgumentException("TStruct must be a value type.");
            }
            int size = Marshal.SizeOf(typeof(TStruct));
            byte[] data = convertEndian ? ReadAndFormat(size) : ReadBytes(size);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(data, 0, ptr, size);
            TStruct i = (TStruct)Marshal.PtrToStructure(ptr, typeof(TStruct));

            if (convertEndian)
            {
                Utils.ConvertStructEndians<TStruct>(ref i);
            }

            Marshal.FreeHGlobal(ptr);
            return i;
        }

        

        private byte[] ReadAndFormat(int count)
        {
            byte[] buffer = new byte[count];
            _stream.Read(buffer, 0, count);
            Utils.ConvertEndian(buffer, _endian);
            return buffer;
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
            if (!_leaveOpen)
            {
                _stream.Dispose();
            }
        }
    }
}
