using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using EasyIO;

namespace StructTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ExampleStruct e = new ExampleStruct(0xFF, 0xFF);

            using(EasyWriter writer = new EasyWriter("example.dat"))
            {
                writer.Write<ExampleStruct>(e);
            }

            Console.WriteLine("Written:");
            Console.WriteLine("A={0}; B={1};", e.FieldA, e.FieldB);

            using(EasyReader reader = new EasyReader("example.dat", FileMode.Open))
            {
                e = reader.ReadStruct<ExampleStruct>();
            }

            Console.WriteLine("Read:");
            Console.WriteLine("A={0}; B={1};", e.FieldA, e.FieldB);

            Console.ReadKey();
        }
    }

    struct ExampleStruct
    {
        [Endianness(Endian.Little)]
        public int FieldA;

        [Endianness(Endian.Big)]
        public int FieldB;

        public ExampleStruct(int a, int b)
        {
            FieldA = a;
            FieldB = b;
        }

        public override string ToString()
        {
            return String.Format("A={0}; B={1};", FieldA, FieldB);
        }
    }
}
