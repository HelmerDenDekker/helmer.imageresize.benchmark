using BenchmarkDotNet.Attributes;
using Helmer.ImageResize.Benchmark.Application.ImageResize;

namespace Helmer.ImageResize.Benchmark.Application;

[CsvMeasurementsExporter]
[MarkdownExporterAttribute.Default]
public class ImageResizeBenchmark
{
    private int size = 150;
    private int quality = 75;

    [Benchmark(Baseline = true)]
    public void ResizeDrawing() => new ImageService().SystemDrawingBenchmark(size, quality);

    [Benchmark]
    public void ResizeImageSharp() => new ImageService().ImageSharpBenchmark(size, quality);

    [Benchmark]
    public void ResizeMagickNet() => new ImageService().MagickNetBenchmark(size, quality);

    [Benchmark]
    public void ResizeMagicScaler() => new ImageService().MagicScalerBenchmark(size, quality);

    [Benchmark]
    public void ResizeSkiaSharp() => new ImageService().SkiaSharpBenchmark(size, quality);

    [Benchmark]
    public void ResizeFreeImage() => new ImageService().FreeImageBenchmark(size, quality);

    [Benchmark]
    public void ResizeImageFlow() => new ImageService().ImageFlowBenchmark(size, quality);

    //[Benchmark]
    //public void ResizeMaui() => new ImageService().MauiBenchmark(size, quality);
}