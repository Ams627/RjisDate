using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RjisDate
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {

                Console.WriteLine($"args: {nameof(args)}");
                var res = new RjisDate(2016, 8, 31);
                var d1 = RjisDate.Parse("29022019", 0);
                var (y, m, d) = d1.GetYmd();
                Console.WriteLine($"date is {y:D4}-{m:D2}-{d:D2}");
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
