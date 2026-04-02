using BenchmarkDotNet.Running;

namespace Helmer.ImageResize.Benchmark.Application;

public class Program
{
    public static void Main(string[] args) => BenchmarkRunner.Run<ImageFormatsBenchmark>(new ShortRunWithMemoryDiagnoserConfig()); 
}