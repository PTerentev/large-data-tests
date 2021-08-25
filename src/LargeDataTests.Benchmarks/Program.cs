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
            BenchmarkRunner.Run<TestDataBenchmark>();
        }
    }
}
