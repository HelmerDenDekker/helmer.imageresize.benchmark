using Helmer.ImageResize.Benchmark.Application.Extensions;
using ImageMagick;

namespace Helmer.ImageResize.Benchmark.Application.ImageResize;

public class ResizeMagickNet
{
    public void ImageResize(int[] sizes, string sourcePath, string destinationPath, int quality)
	{
		foreach (var size in sizes)
		{
			using var image = new MagickImage(sourcePath);
			
            var scaled = SizeLogic.ScaledSize(image.Width, image.Height, size);
            image.Resize((uint)scaled.width, (uint)scaled.height);

            // Reduce the size of the file
            image.Strip();

            // Save the results
            var fileName = FileNameLogic.OutputPath(sourcePath, destinationPath, $"MagickNET-{size}");

			// Set the quality
			image.Quality = (uint)quality;
			image.Write($"{fileName}.jpg", MagickFormat.Jpg);
        }
	}
}