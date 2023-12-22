using FreeImageAPI;
using Helmer.ImageResize.Benchmark.Application.Extensions;

namespace Helmer.ImageResize.Benchmark.Application.ImageResize;

/// <summary>
/// Free Image nuget https://github.com/matgr1/FreeImage-dotnet-core
/// </summary>
public class ResizeFreeImage
{
    // Filepath for saving pictures from settings //ToDo think about width and height
    public void ImageResize(int size, string sourcePath, string destinationPath, int quality)
    {
        

        using (var image = FreeImageBitmap.FromFile(sourcePath))
        {
            var scaled = SizeLogic.ScaledSize(image.Width, image.Height, size);
			using (var resized = new FreeImageBitmap(image, scaled.width, scaled.height))
			{
				// JPEG_QUALITYGOOD is 75 JPEG.
				// JPEG_BASELINE strips metadata (EXIF, etc.)
				resized.Save(FileNameLogic.OutputPath(sourcePath, destinationPath, "FreeImage"), FREE_IMAGE_FORMAT.FIF_JPEG,
					FREE_IMAGE_SAVE_FLAGS.JPEG_QUALITYGOOD |
					FREE_IMAGE_SAVE_FLAGS.JPEG_BASELINE);
			}
        }
    }
}