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
            var dictIn = new Dictionary<int, string>();
            dictIn.Add(1, "One");
            dictIn.Add(2, "Two");
            dictIn.Add(3, "Three");
            dictIn.Add(4, "Four");

            Console.WriteLine("Writing data...");
            using (EasyWriter writer = new EasyWriter("example.dat"))
            {
                writer.Write(dictIn);
            }

            Console.WriteLine("Reading data...");
            using (EasyReader reader = new EasyReader("example.dat"))
            {
                var dictOut = reader.ReadDictionary<int, string>();
                foreach(var pair in dictOut)
                {
                    Console.WriteLine(pair);
                }
            }

            Console.ReadKey();
        }
    }
}
