using Helmer.ImageResize.Benchmark.Application.Extensions;
using SkiaSharp;

namespace Helmer.ImageResize.Benchmark.Application.ImageFormats;
/// <summary>
/// https://schwabencode.com/blog/2019/06/11/Resize-Image-NET-Core Skia was made to be fast
/// </summary>
public class ResizeSkiaSharp
{
    public void ImageResize(int[] sizes, string sourcePath, string destinationPath, int quality)
	{
		//using var original = SKBitmap.Decode(sourcePath); 
		using var skData = SKData.Create(sourcePath);
		using var codec = SKCodec.Create(skData);

		foreach (var size in sizes)
		{
			var scaled = SizeLogic.ScaledSize(codec.Info.Width, codec.Info.Height, size);
			
			// trick for boosting performance?
			var approximatedScale = codec.GetScaledDimensions( (float)scaled.width / codec.Info.Width);
			
			var approximation = new SKImageInfo(approximatedScale.Width, approximatedScale.Height);
			//Resizing via SKImage is blurry. Resizing via SKBitmap is great.
			using var destination = SKBitmap.Decode(codec, approximation);
			var scale = new SKImageInfo(scaled.width, scaled.height);
			var samplingOptions = new SKSamplingOptions(SKCubicResampler.Mitchell);

			using var resized = destination.Resize(scale, samplingOptions);

			if (resized == null)
			{
				return;
			}
			
			using var image = SKImage.FromBitmap(resized);
			
			
			var fileName = FileNameLogic.OutputPath(sourcePath, destinationPath, $"SkiaSharp-{size}");
			
			
			using var pngOutput = File.OpenWrite($"{fileName}.png");
			image.Encode(SKEncodedImageFormat.Png, quality).SaveTo(pngOutput);
			
			using var webpOutput = File.OpenWrite($"{fileName}.webp");
			image.Encode(SKEncodedImageFormat.Webp, quality).SaveTo(webpOutput);
			
			using var jpegOutput = File.OpenWrite($"{fileName}.jpg");
			image.Encode(SKEncodedImageFormat.Jpeg, quality)
				.SaveTo(jpegOutput);
		}
	}
}