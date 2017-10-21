using Pixels2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixels2Console
{
    class Program
    {
        static void Main(string[] args)
        {
            string filename = @"C:\Users\PC\Desktop\000.bin";


            var p = new Pixel<double>(10000, 10000);

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();



            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            Console.ReadKey();
        }
    }
}
