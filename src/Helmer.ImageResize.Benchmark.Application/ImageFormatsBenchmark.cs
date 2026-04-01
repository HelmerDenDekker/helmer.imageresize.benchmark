using BenchmarkDotNet.Attributes;
using Helmer.ImageResize.Benchmark.Application.ImageFormats;

namespace Helmer.ImageResize.Benchmark.Application;

[CsvMeasurementsExporter]
[MarkdownExporterAttribute.Default]
public class ImageFormatsBenchmark
{
    private readonly int[] _sizes = [80, 320, 768];//[80, 320, 768, 1280];
    private readonly int _quality = 95;

    [Benchmark(Baseline = true)]
    public void ResizeDrawing() => new ImageFormatService().SystemDrawingBenchmark(_sizes, _quality);

    [Benchmark]
    public void ResizeImageSharp() => new ImageFormatService().ImageSharpBenchmark(_sizes, _quality);

    [Benchmark]
    public void ResizeMagickNet() => new ImageFormatService().MagickNetBenchmark(_sizes, _quality);

    [Benchmark]
    public void ResizeMagicScaler() => new ImageFormatService().MagicScalerBenchmark(_sizes, _quality);

    [Benchmark]
    public void ResizeNetVips() => new ImageFormatService().VipsBenchmark(_sizes, _quality);
    
    [Benchmark]
    public void ResizeSkiaSharp() => new ImageFormatService().SkiaSharpBenchmark(_sizes, _quality);
}