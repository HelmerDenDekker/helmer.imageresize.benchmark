using Helmer.ImageResize.Benchmark.Application.Extensions;
using ImageMagick;

namespace Helmer.ImageResize.Benchmark.Application.ImageFormats;

public class ResizeMagickNet
{
	//ToDo Check if the images are correctly saved
    public void ImageResize(int[] sizes, string sourcePath, string destinationPath, int quality)
	{
		var settings = new MagickReadSettings
		{
			ColorSpace = ColorSpace.sRGB
		};
		foreach (var size in sizes)
		{
			using var image = new MagickImage(sourcePath, settings);
			
			image.TransformColorSpace(ColorProfiles.SRGB);
			
			var scaled = SizeLogic.ScaledSize(image.Width, image.Height, size);
			//TODO filter-choosing-magic?
			image.Resize((uint)scaled.width, (uint)scaled.height);

			//if(image.ColorSpace != ColorSpace.sRGB)
			//{
			//    image.GammaCorrect(2.2);
			//}
			
			// Reduce the size of the file
			// image.Strip();
			var exif = image.GetExifProfile();
			if (exif != null)
			{
				image.RemoveProfile(exif);
			}
			
            // Save the results
			var fileName = FileNameLogic.OutputPath(sourcePath, destinationPath, $"MagickNET-{size}");
			
			using var png = image.Clone();
			png.Write($"{fileName}.png", MagickFormat.Png);

			using var webp = image.Clone();
			webp.Write($"{fileName}.webp", MagickFormat.WebP);

			// Set the quality
			image.Quality = (uint)quality;
			
			image.Write($"{fileName}.jpg", MagickFormat.Jpg);
        }
	}
}