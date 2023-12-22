using Helmer.ImageResize.Domain.Extensions;
using ImageMagick;

namespace Helmer.ImageResize.Domain;

public class ResizeMagickNet
{
	public void ImageResize(int size, string sourcePath, string destinationPath, int quality)
    {
		using (var image = new MagickImage(sourcePath))
		{
            var scaled = SizeLogic.ScaledSize(image.Width, image.Height, size);
            image.Resize(scaled.width, scaled.height);

			// Reduce the size of the file
			image.Strip();

			// Set the quality
			image.Quality = quality;

			// Save the results
			image.Write(FileNameLogic.OutputPath(sourcePath, destinationPath, "MagickNET"));
		}
    }
}