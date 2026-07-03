using Helmer.ImageResize.Benchmark.Application.Extensions;
using ImageMagick.Drawing;
using NetVips;

namespace Helmer.ImageResize.Benchmark.Application.ImageFormats;

public class ResizeNetVips
{
	public void ImageResize(int[] sizes, string sourcePath, string destinationPath, int quality)
	{
		using var image = Image.NewFromFile(sourcePath);
		foreach (var size in sizes)
		{
			var (width, height) = SizeLogic.ScaledSize(image.Width, image.Height, size);
			// var horizontalShrinkFactor = image.Width / (double)width;
			// var verticalShrinkFactor = image.Height / (double)height;
			using var resized = Image.Thumbnail(sourcePath, width, height); //https://www.libvips.org/API/current/libvips-resample.html
			//using var resized = image.Reduce(horizontalShrinkFactor, verticalShrinkFactor, kernel: Enums.Kernel.Lanczos3);
			// double scale = image.Width / (double)size;
			// var kernel = scale < 1 ? Enums.Kernel.Lanczos3 : Enums.Kernel.Mitchell; // Mitchell is best for enlarging, and shrinking pictures with sharp lines. Otherwise use Lanczos for shrinking.
			// using var resized = image.Resize(scale, kernel);
			
            var fileName = FileNameLogic.OutputPath(sourcePath, destinationPath, $"NetVips-{size}");

			using var imageNoExif = resized.Mutate(mut =>
				{
					foreach (var field in resized.GetFields())
					{
						if (field == "icc-profile-data")
							continue;
						mut.Remove(field);
					}
				}
			);

			using var copy = imageNoExif.CopyMemory();
			
			// save as png
			var pngFileName = $"{fileName}.png";
			copy.WriteToFile(pngFileName);
			
			// save as webp
			var webpFileName = $"{fileName}.webp";
            var webpOptions = new VOption
            {
                { "Q", quality }
            };
            copy.WriteToFile(webpFileName, webpOptions);
			
			// save as jpg
			var jpgFileName = $"{fileName}.jpg";
			var kwargs = new VOption
			{
				{"Q", quality}
			};
			copy.WriteToFile(jpgFileName, kwargs);
		}
	}
}
