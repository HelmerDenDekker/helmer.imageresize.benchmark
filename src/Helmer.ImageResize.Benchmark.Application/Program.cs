using Helmer.ImageResize.Benchmark.Application.ImageResize;

namespace Helmer.ImageResize.Benchmark.Application;

public class Program
{
	public static void Main(string[] args) => new ImageService().MauiBenchmark(150,75);// BenchmarkRunner.Run<ImageResizeBenchmark>(new ShortRunWithMemoryDiagnoserConfig()); 
	//BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
}