using Helmer.ImageResize.Benchmark.Application.Extensions;
using SkiaSharp;

namespace Helmer.ImageResize.Benchmark.Application.ImageResize;
/// <summary>
/// https://schwabencode.com/blog/2019/06/11/Resize-Image-NET-Core Skia was made to be fast
/// </summary>
public class ResizeSkiaSharp
{
    public void ImageResize(int[] sizes, string sourcePath, string destinationPath, int quality)
	{
		using var original = SKBitmap.Decode(sourcePath);

		foreach (var size in sizes)
		{
			var scaled = SizeLogic.ScaledSize(original.Width, original.Height, size);

			using var resized = original.Resize(new SKImageInfo(scaled.width, scaled.height), SKFilterQuality.High);

			if (resized == null)
			{
				return;
			}

			using var image = SKImage.FromBitmap(resized);
			
			var fileName = FileNameLogic.OutputPath(sourcePath, destinationPath, $"SkiaSharp-{size}");
			using var jpegOutput = File.OpenWrite($"{fileName}.jpg");
			using var pngOutput = File.OpenWrite($"{fileName}.png");
			using var webpOutput = File.OpenWrite($"{fileName}.webp");

            image.Encode(SKEncodedImageFormat.Png, quality).SaveTo(pngOutput);
			image.Encode(SKEncodedImageFormat.Webp, quality).SaveTo(webpOutput);
            image.Encode(SKEncodedImageFormat.Jpeg, quality)
				.SaveTo(jpegOutput);
		}
	}
}