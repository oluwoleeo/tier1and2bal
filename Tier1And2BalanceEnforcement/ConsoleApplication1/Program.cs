using System;
using Tier1And2BalanceEnforcement;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime stime = DateTime.Now;
            Console.WriteLine($"Application start: {stime}");

            Notify.InitProcess();

            DateTime etime = DateTime.Now;
            Console.WriteLine($"Application end: {etime}");

            TimeSpan diff = etime - stime;
            Console.WriteLine($"Application ran for {diff.TotalMinutes} minutos");
        }
    }
}
