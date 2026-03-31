using BenchmarkDotNet.Attributes;
using Helmer.ImageResize.Benchmark.Application.ImageResize;

namespace Helmer.ImageResize.Benchmark.Application;

[CsvMeasurementsExporter]
[MarkdownExporterAttribute.Default]
public class ImageResizeBenchmark
{
    private readonly int[] _sizes = [80, 320, 768];//[80, 320, 768, 1280];
    private readonly int _quality = 95;

    [Benchmark(Baseline = true)]
    public void ResizeDrawing() => new ImageService().SystemDrawingBenchmark(_sizes, _quality);

    [Benchmark]
    public void ResizeImageSharp() => new ImageService().ImageSharpBenchmark(_sizes, _quality);

    [Benchmark]
    public void ResizeMagickNet() => new ImageService().MagickNetBenchmark(_sizes, _quality);

    [Benchmark]
    public void ResizeMagicScaler() => new ImageService().MagicScalerBenchmark(_sizes, _quality);

    //[Benchmark]
    //public void ResizeSkiaSharp() => new ImageService().SkiaSharpBenchmark(sizes, quality);

    [Benchmark]
    public void ResizeNetVips() => new ImageService().VipsBenchmark(_sizes, _quality);
    
    [Benchmark]
    public void ResizeSkiaJeveSharp() => new ImageService().SkiaSharpJeveBenchmark(_sizes, _quality);
    
    [Benchmark]
    public void ResizeFreeImage() => new ImageService().FreeImageBenchmark(_sizes, _quality);

    [Benchmark]
    public void ResizeImageFlow() => new ImageService().ImageFlowBenchmark(_sizes, _quality);

    //[Benchmark]
    //public void ResizeMaui() => new ImageService().MauiBenchmark(size, quality);
}