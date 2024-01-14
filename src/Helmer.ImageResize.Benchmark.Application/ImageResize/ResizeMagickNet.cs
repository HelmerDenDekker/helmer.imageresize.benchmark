using Helmer.ImageResize.Benchmark.Application.Extensions;
using ImageMagick;

namespace Helmer.ImageResize.Benchmark.Application.ImageResize;

public class ResizeMagickNet
{
	//ToDo MagickNET destroys images and does not save three formats
    public void ImageResize(int[] sizes, string sourcePath, string destinationPath, int quality)
	{
		foreach (var size in sizes)
		{
			using var image = new MagickImage(sourcePath);
            var scaled = SizeLogic.ScaledSize(image.Width, image.Height, size);
			image.Resize(scaled.width, scaled.height);

			// Reduce the size of the file
			image.Strip();

            // Save the results
			var fileName = FileNameLogic.OutputPath(sourcePath, destinationPath, $"MagickNET-{size}");
            image.Write($"{fileName}.png", MagickFormat.Png);
			image.Write($"{fileName}.webp", MagickFormat.WebP);

			// Set the quality
			image.Quality = quality;
			image.Write($"{fileName}.jpg", MagickFormat.Jpg);
        }
	}
}