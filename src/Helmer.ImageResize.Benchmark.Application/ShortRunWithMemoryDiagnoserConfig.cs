using System.Runtime.InteropServices;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Filters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
#if Windows_NT
using System.Security.Principal;
using BenchmarkDotNet.Diagnostics.Windows;
#endif

namespace Helmer.ImageResize.Benchmark.Application;

public class ShortRunWithMemoryDiagnoserConfig : ManualConfig
{
    public ShortRunWithMemoryDiagnoserConfig()
    {
        this.AddJob(Job.ShortRun
            .WithWarmupCount(5)
            .WithIterationCount(5)
            .WithArguments(new Argument[]
            {
				// See https://github.com/dotnet/roslyn/issues/42393
				new MsBuildArgument("/p:DebugType=portable")
            }));

        this.AddColumnProvider(DefaultColumnProviders.Instance);
        this.AddLogger(ConsoleLogger.Default);
        this.AddExporter(MarkdownExporter.GitHub);
        this.AddDiagnoser(MemoryDiagnoser.Default);

        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // MagicScaler requires Windows Imaging Component (WIC) which is only available on Windows
            this.AddFilter(new NameFilter(name => !name.StartsWith("MagicScalerBenchmark")));
        }

        if (RuntimeInformation.OSArchitecture is not (Architecture.X86 or Architecture.X64))
        {
            // ImageMagick native binaries are currently only available for X86 and X64
            this.AddFilter(new NameFilter(name => !name.StartsWith("Magick")));
        }

#if Windows_NT
            // See https://github.com/microsoft/perfview/issues/1264
            if (this.IsElevated && RuntimeInformation.OSArchitecture != Architecture.Arm64)
            {
                this.AddDiagnoser(new NativeMemoryProfiler());
            }
#endif
    }
}
