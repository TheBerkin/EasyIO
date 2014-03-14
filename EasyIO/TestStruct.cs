using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyIO
{
    struct TestStruct
    {
        [Endianness(Endian.Big)]
        public int NumberA;

        [Endianness(Endian.Little)]
        public int NumberB;
    }
}
