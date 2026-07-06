using Helmer.ImageResize.Benchmark.Application.Extensions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace Helmer.ImageResize.Benchmark.Application.ImageFormats;

// https://docs.sixlabors.com/articles/imagesharp/resize.html
// 420 color sampling
// Lanczos3
public class ResizeImageSharp
{
    public void ImageResize(int[] sizes, string sourcePath, string destinationPath, int quality)
    {
		using var image = Image.Load(sourcePath);
		foreach (var size in sizes)
		{
			var (width, height) = SizeLogic.ScaledSize(image.Width, image.Height, size);
			using Image resized = image.Clone(i => i.Resize(width, height, KnownResamplers.MitchellNetravali));

			// Reduce the size of the file //ToDo is this cheating??
			resized.Metadata.ExifProfile = null;
			resized.Metadata.IptcProfile = null;
			resized.Metadata.XmpProfile = null;
			
			var fileName = FileNameLogic.OutputPath(sourcePath, destinationPath, $"ImageSharp-{size}");
			
			// Save the results.
			using var jpegOutput = File.Open($"{fileName}.jpg", FileMode.Create);
			var imageSharpJpegEncoder = new JpegEncoder() { Quality = quality, ColorType = JpegEncodingColor.YCbCrRatio420 }; // 420 is medium quality
			resized.Save(jpegOutput, imageSharpJpegEncoder);
			
			using var pngOutput = File.Open($"{fileName}.png", FileMode.Create);
			var pngEncoder = new PngEncoder();
			resized.Save(pngOutput, pngEncoder);
			
			using var webpOutput = File.Open($"{fileName}.webp", FileMode.Create);
			var webpEncoder = new WebpEncoder() { Quality = quality };
			resized.Save(webpOutput, webpEncoder);
		}
	}
}