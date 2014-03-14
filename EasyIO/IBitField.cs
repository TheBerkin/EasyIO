using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyIO
{
    interface IBitField
    {
        bool this[int i]
        {
            get;
            set;
        }


        void UnsetAll();
        void SetAll();

        int GetSetCount();
        int GetUnsetCount();

        void Invert();

        byte[] GetBytes();
    }
}
