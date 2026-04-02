using Helmer.ImageResize.Benchmark.Application.Extensions;
using SkiaSharp;

namespace Helmer.ImageResize.Benchmark.Application.ImageResize;
/// <summary>
/// https://schwabencode.com/blog/2019/06/11/Resize-Image-NET-Core Skia was made to be fast // This is based on the original https://github.com/bleroy/core-imaging-playground/blob/master/NetCore/LoadResizeSave.cs
/// https://github.com/kleisauke/net-vips/blob/master/tests/NetVips.Benchmarks/Benchmark.cs
/// </summary>
public class ResizeSkiaSharp
{
    public void ImageResize(int[] sizes, string sourcePath, string destinationPath, int quality)
	{
		using var original = SKBitmap.Decode(sourcePath); 
		
		foreach (var size in sizes)
		{
			var scaled = SizeLogic.ScaledSize(original.Width, original.Height, size);

			using var copy = original.Copy();

            var scale = new SKImageInfo(scaled.width, scaled.height);
			var samplingOptions = new SKSamplingOptions(SKFilterMode.Linear); // Mitchell is high quality for upsampling, Linear for downsampling

			using var resized = copy.Resize(scale, samplingOptions);

			if (resized == null)
			{
				return;
			}
			
			// using var image = SKImage.FromBitmap(resized);
			//
			//
			var fileName = FileNameLogic.OutputPath(sourcePath, destinationPath, $"SkiaSharp-{size}.jpg");
			//
			// using var jpegOutput = File.OpenWrite($"{fileName}.jpg");
			// image.Encode(SKEncodedImageFormat.Jpeg, quality)
			// 	.SaveTo(jpegOutput);
			
			using var surface =
				SKSurface.Create(new SKImageInfo(scaled.width, scaled.height, original.ColorType, original.AlphaType));
			using var canvas = surface.Canvas;
			using var paint = new SKPaint();
			paint.IsAntialias = true;
			canvas.DrawBitmap(resized, 0, 0, paint);
			canvas.Flush();
			using var fileStream = File.OpenWrite(fileName);
			surface.Snapshot()
				.Encode(SKEncodedImageFormat.Jpeg, quality)
				.SaveTo(fileStream);
		}
	}
}