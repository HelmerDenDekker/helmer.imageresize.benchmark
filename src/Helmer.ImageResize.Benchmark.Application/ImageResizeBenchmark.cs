using BenchmarkDotNet.Attributes;
using Helmer.ImageResize.Benchmark.Application.ImageResize;

namespace Helmer.ImageResize.Benchmark.Application;

[CsvMeasurementsExporter]
[MarkdownExporterAttribute.Default]
public class ImageResizeBenchmark
{
    private int[] sizes = [80, 320, 768, 1280];
    private int quality = 95;

    [Benchmark(Baseline = true)]
    public void ResizeDrawing() => new ImageService().SystemDrawingBenchmark(sizes, quality);

    [Benchmark]
    public void ResizeImageSharp() => new ImageService().ImageSharpBenchmark(sizes, quality);

    [Benchmark]
    public void ResizeMagickNet() => new ImageService().MagickNetBenchmark(sizes, quality);

    [Benchmark]
    public void ResizeMagicScaler() => new ImageService().MagicScalerBenchmark(sizes, quality);

    [Benchmark]
    public void ResizeSkiaSharp() => new ImageService().SkiaSharpBenchmark(sizes, quality);

    //[Benchmark]
    //public void ResizeFreeImage() => new ImageService().FreeImageBenchmark(sizes, quality);

    [Benchmark]
    public void ResizeImageFlow() => new ImageService().ImageFlowBenchmark(sizes, quality);

    //[Benchmark]
    //public void ResizeMaui() => new ImageService().MauiBenchmark(size, quality);
}