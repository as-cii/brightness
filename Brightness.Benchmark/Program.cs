using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brightness.Benchmark
{
    class Program
    {
        class Employee
        {
            public int Id { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("======== BRIGHTNESS BENCHMARK =======");
            Console.WriteLine("========  -ANTONIO SCANDURRA- =======");

            var sw = Stopwatch.StartNew();
            var differences = DeltaEngine.Diff("old.dat", "new.dat", (Employee e) => e.Id);
            sw.Stop();

            Console.WriteLine("Processed 500000 rows in {0}ms", sw.ElapsedMilliseconds);
            Console.WriteLine("Press ENTER to exit...");
            Console.ReadLine();
        }
    }
}
