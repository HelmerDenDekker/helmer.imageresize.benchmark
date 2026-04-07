using BenchmarkDotNet.Attributes;
using Helmer.ImageResize.Benchmark.Application.Array;

namespace Helmer.ImageResize.Benchmark.Application
{
    public class ArrayBenchmark
	{
		private System.Guid _guid = System.Guid.NewGuid();


		[Benchmark(Baseline = true)]
		public void TestWithArray() => ArrayTest.TestWithArray();

		[Benchmark]
		public void GuidTextEnTestwithoutArray() =>ArrayTest.TestWithoutArray();
	}
}
