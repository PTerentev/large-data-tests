using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Text;

namespace LargeDataTests.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<TestDataJsonBenchmark>();

            //TestDataProcessingWatcher.Run(10000);
            //TestDataProcessingWatcher.Run(100000);

            //Console.ReadKey();
        }
    }
}
