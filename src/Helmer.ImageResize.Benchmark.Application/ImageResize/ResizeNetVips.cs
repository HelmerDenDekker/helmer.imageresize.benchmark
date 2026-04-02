using Helmer.ImageResize.Benchmark.Application.Extensions;
using NetVips;

namespace Helmer.ImageResize.Benchmark.Application.ImageResize;

public class ResizeNetVips
{
	public void ImageResize(int[] sizes, string sourcePath, string destinationPath, int quality)
	{
		using var image = Image.NewFromFile(sourcePath);
		foreach (var size in sizes)
		{
			var (width, height) = SizeLogic.ScaledSize(image.Width, image.Height, size);
			var horizontalShrinkFactor = image.Width / width;
			var verticalShrinkFactor = image.Height / height;
			//using var resized = Image.Thumbnail(sourcePath, width, height);
			using var resized = image.Reduce(horizontalShrinkFactor, verticalShrinkFactor, kernel: Enums.Kernel.Linear);
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
			// save as jpg
			var jpgFileName = $"{fileName}.jpg";
			var kwargs = new VOption
			{
				{"Q", quality}
			};
			imageNoExif.WriteToFile(jpgFileName, kwargs);
		}
	}
}
