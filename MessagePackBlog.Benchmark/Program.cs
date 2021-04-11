using System;
using BenchmarkDotNet.Running;

namespace MessagePackBlog.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            //var summary = BenchmarkRunner.Run<Serialization>();
            var summary = BenchmarkRunner.Run<Deserialization>();
        }
    }
}
