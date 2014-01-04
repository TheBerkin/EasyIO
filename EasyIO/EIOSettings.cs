using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyIO
{
    public static class EIOSettings
    {
        private static Encoding _enc = Encoding.ASCII;

        public static Encoding TextEncoding
        {
            get { return _enc; }
            set
            {
                if (value == null)
                {
                    _enc = Encoding.ASCII;
                }
                else
                {
                    _enc = value;
                }
            }
        }
    }
}
