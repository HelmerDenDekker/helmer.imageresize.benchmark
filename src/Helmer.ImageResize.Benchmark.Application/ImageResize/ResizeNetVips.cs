using Helmer.ImageResize.Benchmark.Application.Extensions;
using NetVips;

namespace Helmer.ImageResize.Benchmark.Application.ImageResize;

public class ResizeNetVips
{
	public void ImageResize(int[] sizes, string sourcePath, string destinationPath, int quality)
	{
		using var image = NetVips.Image.NewFromFile(sourcePath);
		foreach (var size in sizes)
		{
			var (width, height) = SizeLogic.ScaledSize(image.Width, image.Height, size);
			using var resized = NetVips.Image.Thumbnail(sourcePath, width, height);
			var fileName = FileNameLogic.OutputPath(sourcePath, destinationPath, $"NetVips-{size}");

			resized.Mutate(mut =>
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
			var target = Target.NewToFile(jpgFileName);
			var kwargs = new VOption
			{
				{"Q", quality}
			};
			resized.WriteToTarget(target, ".jpg", kwargs);
		}
	}
}
