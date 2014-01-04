using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace EasyIO
{
    public static class EIOStrings
    {
        /// <summary>
        /// Pushes a string to the start of a list file and shifts the list contents to the right, deleting the last entry.
        /// </summary>
        /// <param name="path">The path to the file</param>
        /// <param name="str">The string to push.</param>
        /// <returns>True if successful; otherwise, false.</returns>
        public static bool PushStartFixedASCII(string path, string str)
        {
            string[] strs = ReadStringsFromBinary(path);
            int n = strs.Length;
            if (strs.Length == 0)
            {
                return false;
            }

            string[] sout = new string[n];
            sout[0] = str;

            if (n == 1)
            {
                WriteStringsToBinary(path, sout);
                return true;
            }

            for (int i = 1; i < n; i++)
            {
                sout[i] = strs[i - 1];
            }

            WriteStringsToBinary(path, sout);
            return true;
        }

        /// <summary>
        /// Pushes a string to the end of a list file and shifts the list contents to the left, deleting the first entry.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <param name="str">The string to push.</param>
        /// <returns>True if successful; otherwise, false.</returns>
        public static bool PushEndFixedASCII(string path, string str)
        {
            string[] strs = ReadStringsFromBinary(path);
            int n = strs.Length;
            if (strs.Length == 0)
            {
                return false;
            }

            string[] sout = new string[n];
            sout[n - 1] = str;

            if (n == 1)
            {
                WriteStringsToBinary(path, sout);
                return true;
            }

            for (int i = 0; i < n - 1; i++)
            {
                sout[i] = strs[i + 1];
            }

            WriteStringsToBinary(path, sout);
            return true;
        }

        /// <summary>
        /// Reads an array of strings from a binary file.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <returns>The string array read from the file.</returns>
        public static string[] ReadStringsFromBinary(string path)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                int count = reader.ReadInt32();
                string[] strs = new string[count];
                for(int i = 0; i < count; i++)
                {
                    strs[i] = reader.ReadEIOString();
                }
                return strs;
            }
        }

        /// <summary>
        /// Writes an array of strings to a file.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <param name="strs">The array of strings to write to the file.</param>
        public static void WriteStringsToBinary(string path, string[] strs)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Create(path)))
            {
                writer.Write(strs.Length);
                for(int i = 0; i < strs.Length; i++)
                {
                    writer.WriteEIOString(strs[i]);
                }
            }
        }
    }
}
