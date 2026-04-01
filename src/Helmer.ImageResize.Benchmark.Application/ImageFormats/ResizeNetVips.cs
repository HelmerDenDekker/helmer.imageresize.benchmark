using Helmer.ImageResize.Benchmark.Application.Extensions;
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
			using var resized = Image.Thumbnail(sourcePath, width, height);
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
			
			// save as png
			var pngFileName = $"{fileName}.png";
			var pngTarget = Target.NewToFile(pngFileName);
			imageNoExif.WriteToTarget(pngTarget, ".png");
			
			// save as webp
			var webpFileName = $"{fileName}.webp";
			var webpTarget = Target.NewToFile(webpFileName);
			imageNoExif.WriteToTarget(webpTarget, ".webp");
			
			// save as jpg
			var jpgFileName = $"{fileName}.jpg";
			var target = Target.NewToFile(jpgFileName);
			var kwargs = new VOption
			{
				{"Q", quality}
			};
			imageNoExif.WriteToTarget(target, ".jpg", kwargs);
		}
	}
}
