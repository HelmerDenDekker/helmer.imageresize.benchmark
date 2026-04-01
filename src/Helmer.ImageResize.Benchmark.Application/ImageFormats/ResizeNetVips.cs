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
			
			// using var imageNoExif = resized.Mutate(mut =>
			// 	{
			// 		foreach (var field in resized.GetFields())
			// 		{
			// 			if (field == "icc-profile-data")
			// 				continue;
			// 			mut.Remove(field);
			// 		}
			// 	}
			// );
			
			using var copy = resized.CopyMemory();
				
			
			// save as png
			var pngFileName = $"{fileName}.png";
			var pngOptions = new VOption
			{
				{ "strip", true }
			};
			copy.WriteToFile(pngFileName, pngOptions);
			
			// save as webp
			var webpFileName = $"{fileName}.webp";
            var webpOptions = new VOption
            {
                { "Q", quality },
                { "strip", true }
            };
            copy.WriteToFile(webpFileName, webpOptions);
			
			// save as jpg
			var jpgFileName = $"{fileName}.jpg";
			var kwargs = new VOption
			{
				{"Q", quality},
				{ "strip", true }
			};
			copy.WriteToFile(jpgFileName, kwargs);
		}
	}
}
