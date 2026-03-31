using Helmer.ImageResize.Benchmark.Application.Extensions;
using SkiaSharp;

namespace Helmer.ImageResize.Benchmark.Application.ImageResize;
/// <summary>
/// https://schwabencode.com/blog/2019/06/11/Resize-Image-NET-Core Skia was made to be fast // This is based on the blog by Jeroen Verhaeghe https://medium.com/@jeroenverhaeghe/how-to-resize-images-in-dotnet-core-2024-edition-acf6dca09afb
/// </summary>
public class ResizeSkiaSharpJeVe
{
    public void ImageResize(int[] sizes, string sourcePath, string destinationPath, int quality)
	{
		//using var original = SKBitmap.Decode(sourcePath); 
		using var skData = SKData.Create(sourcePath);
		using var codec = SKCodec.Create(skData);

		foreach (var size in sizes)
		{
			var scaled = SizeLogic.ScaledSize(codec.Info.Width, codec.Info.Height, size);

            var sp = codec.Info.ColorSpace;

            // trick for boosting performance?
            var approximatedScale = codec.GetScaledDimensions( (float)scaled.width / codec.Info.Width);
			
			var approximation = new SKImageInfo(approximatedScale.Width, approximatedScale.Height, SKImageInfo.PlatformColorType, SKAlphaType.Premul, SKColorSpace.CreateSrgb());
			//Resizing via SKImage is blurry. Resizing via SKBitmap is great. (???)
			using var destination = SKBitmap.Decode(codec, approximation);
			var scale = new SKImageInfo(scaled.width, scaled.height);
			var samplingOptions = new SKSamplingOptions(SKCubicResampler.Mitchell);

			using var resized = destination.Resize(scale, samplingOptions);

			if (resized == null)
			{
				return;
			}
			
			using var image = SKImage.FromBitmap(resized);
			
			
			var fileName = FileNameLogic.OutputPath(sourcePath, destinationPath, $"SkiaSharpJeVe-{size}");
			
			using var jpegOutput = File.OpenWrite($"{fileName}.jpg");
			image.Encode(SKEncodedImageFormat.Jpeg, quality)
				.SaveTo(jpegOutput);
		}
	}
}