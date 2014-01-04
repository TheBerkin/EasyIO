using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace EasyIO
{
    static class Extensions
    {
        public static string ReadEIOString(this BinaryReader reader)
        {
            int lengthBytes = reader.ReadInt32();
            return EIOSettings.TextEncoding.GetString(reader.ReadBytes(lengthBytes));
        }

        public static void WriteEIOString(this BinaryWriter writer, string data)
        {
            writer.Write(data.GetEIOByteCount());
            writer.Write(data.GetEIOBytes());
        }

        public static int GetEIOByteCount(this string str)
        {
            return EIOSettings.TextEncoding.GetByteCount(str);
        }

        public static byte[] GetEIOBytes(this string str)
        {
            return EIOSettings.TextEncoding.GetBytes(str);
        }

        public static bool GetFlag(this byte field, int pos)
        {
            return ((field >> pos) & 0x1) == 1;
        }

        public static bool GetFlag(this short field, int pos)
        {
            return ((field >> pos) & 0x1) == 1;
        }

        public static bool GetFlag(this int field, int pos)
        {
            return ((field >> pos) & 0x1) == 1;
        }

        public static bool GetFlag(this long field, int pos)
        {
            return ((field >> pos) & 0x1) == 1;
        }
    }
}
