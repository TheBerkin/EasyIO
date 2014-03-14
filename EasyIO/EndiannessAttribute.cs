using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyIO
{
    /// <summary>
    /// Specifies the byte order in which a field should be written and read by EasyWriter/EasyReader.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class EndiannessAttribute : Attribute
    {
        private Endian _endian;

        /// <summary>
        /// The endianness to represent the field data in.
        /// </summary>
        public Endian Endianness
        {
            get { return _endian; }
        }

        /// <summary>
        /// Initializes a new instance of the EasyIO.EndiannessAttribute class with the specified endianness.
        /// </summary>
        /// <param name="endianness">The endianness to represent the field data in.</param>
        public EndiannessAttribute(Endian endianness)
        {
            _endian = endianness;
        }
    }
}
