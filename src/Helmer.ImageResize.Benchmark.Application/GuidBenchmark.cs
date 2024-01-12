using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Helmer.ImageResize.Benchmark.Application.Guid;
using Microsoft.AspNetCore.WebUtilities;

namespace Helmer.ImageResize.Benchmark.Application
{
    public class GuidBenchmark
	{
		private System.Guid _guid = System.Guid.NewGuid();


		[Benchmark(Baseline = true)]
		public void GuidConvert() => Convert.ToBase64String(_guid.ToByteArray()).Replace("/", "-").Replace("+", "_").Replace("=", "");

		[Benchmark]
		public void GuidTextEncode() => Base64UrlTextEncoder.Encode(_guid.ToByteArray());

		[Benchmark]
		public void GuidReneEncode() => _guid.ToString("N");

        [Benchmark]
		public void GuidEncode() => _guid.EncodeBase64String();
	}
}
