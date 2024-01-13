using BenchmarkDotNet.Running;
using Helmer.ImageResize.Benchmark.Application.ImageResize;

namespace Helmer.ImageResize.Benchmark.Application;

public class Program
{
    public static void Main(string[] args) => BenchmarkRunner.Run<ImageResizeBenchmark>(new ShortRunWithMemoryDiagnoserConfig()); // new ImageService().MauiBenchmark(150,75);//
                                                                                                                                  //BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
}