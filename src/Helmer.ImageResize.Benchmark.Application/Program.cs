using BenchmarkDotNet.Running;
using Helmer.ImageResize.Benchmark.Application.ImageResize;

namespace Helmer.ImageResize.Benchmark.Application;

public class Program
{
    public static void Main(string[] args) => BenchmarkRunner.Run<ImageFormatsBenchmark>(new ShortRunWithMemoryDiagnoserConfig()); 
}