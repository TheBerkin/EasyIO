using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace EasyIO
{
    /// <summary>
    /// Provides functions for updating string entries.
    /// </summary>
    public static class EasyStrings
    {
        /// <summary>
        /// Pushes a string to the start of a list file and shifts the list contents to the right, deleting the last entry.
        /// </summary>
        /// <param name="path">The path to the file</param>
        /// <param name="str">The string to push.</param>
        /// <returns>True if successful; otherwise, false.</returns>
        public static bool PushStartFixed(string path, string str)
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
        public static bool PushEndFixed(string path, string str)
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
        
        private static string[] ReadStringsFromBinary(string path)
        {
            using (EasyReader reader = new EasyReader(path))
            {
                return reader.ReadStringArray();
            }
        }

        private static void WriteStringsToBinary(string path, string[] strs)
        {
            using (EasyWriter writer = new EasyWriter(path))
            {
                writer.Write(strs);
            }
        }
    }
}
