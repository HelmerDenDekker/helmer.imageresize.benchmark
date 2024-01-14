using Helmer.ImageResize.Benchmark.Application.Extensions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace Helmer.ImageResize.Benchmark.Application.ImageResize;

// https://docs.sixlabors.com/articles/imagesharp/resize.html
public class ResizeImageSharp
{
    public void ImageResize(int[] sizes, string sourcePath, string destinationPath, int quality)
    {

        var imageSharpJpegEncoder = new JpegEncoder() { Quality = quality, ColorType = JpegEncodingColor.YCbCrRatio420 };
		var pngEncoder = new PngEncoder();
		var webpEncoder = new WebpEncoder();

		using var image = Image.Load(sourcePath);
		foreach (var size in sizes)
		{
			var (width, height) = SizeLogic.ScaledSize(image.Width, image.Height, size);
			using Image resized = image.Clone(i => i.Resize(width, height));

			// Reduce the size of the file //ToDo is this cheating??
			resized.Metadata.ExifProfile = null;
			resized.Metadata.IptcProfile = null;
			resized.Metadata.XmpProfile = null;

			var fileName = FileNameLogic.OutputPath(sourcePath, destinationPath, $"ImageSharp-{size}");
			using var jpegOutput = File.Open($"{fileName}.jpg", FileMode.Create);
			using var pngOutput = File.Open($"{fileName}.png", FileMode.Create);
			using var webpOutput = File.Open($"{fileName}.webp", FileMode.Create);

            // Save the results. //ToDo does not save multople files! It saves broken stuff instead!
            resized.Save(pngOutput, pngEncoder);
			resized.Save(webpOutput, webpEncoder);
			resized.Save(jpegOutput, imageSharpJpegEncoder);
		}
	}
}