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
            TestStruct test = new TestStruct()
            {
                A = 1,
                B = 2,
                C = 3
            };

            Console.WriteLine("Writing data...");
            using (EasyWriter writer = new EasyWriter("example.dat"))
            {
                writer.Write(TestEnum.Foo);
                writer.Write(TestEnum.Bar);
                writer.Write(test);
            }

            Console.WriteLine("Reading data...");
            using (EasyReader reader = new EasyReader("example.dat"))
            {
                Console.WriteLine("TestEnum.{0}", reader.ReadEnum<TestEnum>());
                Console.WriteLine("TestEnum.{0}", reader.ReadEnum<TestEnum>());
                Console.WriteLine(reader.ReadStruct<TestStruct>());
            }

            Console.ReadKey();
        }

        struct TestStruct
        {
            public int A;

            [Endianness(Endian.Big)]
            public long B;

            public float C;

            public override string ToString()
            {
                return String.Format("TestStruct: A={0}; B={1}; C={2};", A, B, C);
            }
        }

        enum TestEnum : int
        {
            Foo = 1,
            Bar = 2
        }
    }
}
