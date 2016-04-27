using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neuron
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Neural Net Demo\n");


            var sw = new Stopwatch();
            sw.Start();

            Console.Write("Training Logical OR...  ");
            var netOR = Training.Helper.When(1, 0).Then(1).When(1, 1).Then(1).When(0, 1).Then(1).When(0, 0).Then(0).Train();
            Console.Write("Training Logical AND... ");
            var netAND = Training.Helper.When(1, 0).Then(0).When(1, 1).Then(1).When(0, 1).Then(0).When(0, 0).Then(0).Train(2);
            Console.Write("Training Logical XOR... ");
            var netXOR = Training.Helper.When(1, 0).Then(1).When(1, 1).Then(0).When(0, 1).Then(1).When(0, 0).Then(0).Train(2,2);
            Console.Write("Training Logical NOT... ");
            var netNOT = Training.Helper.When(1).Then(0).When(0).Then(1).Train();

            sw.Stop();
            Console.WriteLine("Training Time: " + (sw.ElapsedMilliseconds/1000) + "s\n");
            Console.WriteLine("Results:");
            Console.WriteLine();
            Console.WriteLine("     1 OR 1 = " + netOR.Solve(1, 1)[0]);
            Console.WriteLine("     1 OR 0 = " + netOR.Solve(1, 0)[0]);
            Console.WriteLine("     0 OR 1 = " + netOR.Solve(0, 1)[0]);
            Console.WriteLine("     0 OR 0 = " + netOR.Solve(0, 0)[0]);
            Console.WriteLine();
            Console.WriteLine("    1 AND 1 = " + netAND.Solve(1, 1)[0]);
            Console.WriteLine("    1 AND 0 = " + netAND.Solve(1, 0)[0]);
            Console.WriteLine("    0 AND 1 = " + netAND.Solve(0, 1)[0]);
            Console.WriteLine("    0 AND 0 = " + netAND.Solve(0, 0)[0]);
            Console.WriteLine();
            Console.WriteLine("    1 XOR 1 = " + netXOR.Solve(1, 1)[0]);
            Console.WriteLine("    1 XOR 0 = " + netXOR.Solve(1, 0)[0]);
            Console.WriteLine("    0 XOR 1 = " + netXOR.Solve(0, 1)[0]);
            Console.WriteLine("    0 XOR 0 = " + netXOR.Solve(0, 0)[0]);
            Console.WriteLine();
            Console.WriteLine("      NOT 0 = " + netNOT.Solve(0)[0]);
            Console.WriteLine("      NOT 1 = " + netNOT.Solve(1)[0]);

            if (System.Diagnostics.Debugger.IsAttached) Console.Read();
        }
    }
}
