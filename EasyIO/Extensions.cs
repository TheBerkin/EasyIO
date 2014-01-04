using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EasyIO
{
    public static class Extensions
    {
        public static void Write(this BinaryWriter writer, Bitfield8 bf)
        {
            writer.Write(bf.GetByte());
        }

        public static Bitfield8 ReadBitfield8(this BinaryReader reader)
        {
            return new Bitfield8(reader.ReadByte());
        }
    }
}
