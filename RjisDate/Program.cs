using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RjisDate
{
    class RjisDate
    {
        static int[] MonthLengths = new[] { -1, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        private UInt16 serial;
        public RjisDate(int y, int m, int d)
        {
            var serial32 = GetSerial(y, m, d);
            if ((serial32 & 0xFFFF0000) > 0)
            {
                serial = 0xFFFF;
            }
            else
            {
                serial = (UInt16)(serial32 & 0xFFFF);
            }
        }
        private static UInt32 GetSerial(int y, int m, int d)
        {
            int result = 367 * y - 7 * (y + (m + 9) / 12) / 4 - 3 * ((y + (m - 9) / 7) / 100 + 1) / 4 + 275 * m / 9 + d - 736360;
            return (UInt32)result;
        }

         void GetYMD(out int y, out int m, out int d, int serial)
         {
            int j = serial + 719469 + 16801;
            y = (4 * j - 1) / 146097;
            j = (4 * j - 1) % 146097;
            d = j / 4;
            j = (4 * d + 3) / 1461;
            d = ((4 * d + 3) % 1461) / 4 + 1;
            m = (5 * d - 3) / 153;
            d = ((5 * d - 3) % 153) / 5 + 1;
            y = 100 * y + j;
            if (m < 10)
            {
                m = m + 3;
            }
            else
            {
                m = m - 9;
                y = y + 1;
            }
        }

        void Parse(string line, int offset)
        {
            if (line.Length - offset < 8)
            {
                throw new Exception("Cannot parse date string as the string is too short.");
            }
            for (int i = 0; i < 8; i++)
            {
                var c = line[i + offset];
                if (!char.IsDigit(c))
                {
                    throw new Exception($"Invalid character in date: '{c}'");
                }
            }
            int d = 10 * (line[offset] - '0') + (line[offset + 1] - '0');
            int m = 10 * (line[offset + 2] - '0') + (line[offset + 3] - '0');
            int y = 1000 * (line[offset + 4] - '0') + 100 * (line[offset + 5] - '0') + 10 * (line[offset + 6] - '0') + (line[offset + 7] - '0');
            if (m == 0 || m > 12)
            {
                throw new ArgumentOutOfRangeException($"Month {m} is out of range - must be 1-12.");
            }
            if (y < 1970 || y > 2999)
            {
                throw new ArgumentOutOfRangeException($"Year {y} is out of range - must be 1970-2999.");
            }

            var maxDays = MonthLengths[m];

            // leap year - don't need full algorithm as we have a limited date range:
            var isLeapYear = y % 4 == 0;
            if (isLeapYear && m == 2)
            {
                maxDays = 29;
            }

            if (d == 0 || d > maxDays)
            {
                throw new ArgumentOutOfRangeException($"Day {d} is out of range - must be 0-{maxDays}.");
            }

            if (y > 2194)
            {
                serial = 0xFFFF;
            }
            else if (y < 2016)
            {
                y = 0;
            }
            serial = (UInt16)GetSerial(y, m, d);
        }
        public  (int y, int m, int d) GetYmd()
        {
            GetYMD(out int y, out int m, out int d, serial);
            return (y, m, d);
        }
    }
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var res = new RjisDate(2016, 8, 31);
                var 
                var (y, m, d) = res.GetYmd();
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                var codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                var progname = Path.GetFileNameWithoutExtension(codeBase);
                Console.Error.WriteLine(progname + ": Error: " + ex.Message);
            }

        }
    }
}
