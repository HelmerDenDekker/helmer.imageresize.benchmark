using Helmer.ImageResize.Benchmark.Application.Extensions;
using ImageMagick;

namespace Helmer.ImageResize.Benchmark.Application.ImageResize;

public class ResizeMagickNet
{
    public void ImageResize(int[] sizes, string sourcePath, string destinationPath, int quality)
    {

		using (var image = new MagickImage(sourcePath))
		{
			foreach (var size in sizes)
			{
				var scaled = SizeLogic.ScaledSize(image.Width, image.Height, size);
				image.Resize(scaled.width, scaled.height);

				// Reduce the size of the file
				image.Strip();

				// Set the quality
				image.Quality = quality;

				// Save the results
				image.Write(FileNameLogic.OutputPath(sourcePath, destinationPath, $"MagickNET-{size}"));
			}
		}
	}
}