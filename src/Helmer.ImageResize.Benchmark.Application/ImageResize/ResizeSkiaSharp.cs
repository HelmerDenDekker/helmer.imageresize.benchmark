using Helmer.ImageResize.Benchmark.Application.Extensions;
using SkiaSharp;

namespace Helmer.ImageResize.Benchmark.Application.ImageResize;
/// <summary>
/// https://schwabencode.com/blog/2019/06/11/Resize-Image-NET-Core Skia was made to be fast // This is based on the original https://github.com/bleroy/core-imaging-playground/blob/master/NetCore/LoadResizeSave.cs
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
			var samplingOptions = new SKSamplingOptions(SKCubicResampler.Mitchell); // Mitchell is high quality

			using var resized = copy.Resize(scale, samplingOptions);

			if (resized == null)
			{
				return;
			}
			
			using var image = SKImage.FromBitmap(resized);
			
			
			var fileName = FileNameLogic.OutputPath(sourcePath, destinationPath, $"SkiaSharp-{size}");
			
			using var jpegOutput = File.OpenWrite($"{fileName}.jpg");
			image.Encode(SKEncodedImageFormat.Jpeg, quality)
				.SaveTo(jpegOutput);
		}
	}
}