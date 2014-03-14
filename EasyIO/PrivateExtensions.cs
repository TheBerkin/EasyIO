using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace EasyIO
{
    internal static class PrivateExtensions
    {
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
