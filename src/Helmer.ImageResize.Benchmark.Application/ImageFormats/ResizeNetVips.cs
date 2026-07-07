using Helmer.ImageResize.Benchmark.Application.Extensions;
using ImageMagick.Drawing;
using NetVips;

namespace Helmer.ImageResize.Benchmark.Application.ImageFormats;

public class ResizeNetVips
{
	//https://github.com/kleisauke/net-vips/blob/516abb12/samples/NetVips.Samples/Samples/ThumbnailPipeline.cs#L126-L138
	public void ImageResize(int[] sizes, string sourcePath, string destinationPath, int quality)
	{
		using var image = Image.NewFromFile(sourcePath);
		
		// srgb is the standard color profile for browsers.
		string? inputProfile = null;
		string? outputProfile = null;
		Enums.Intent? intent = null;

		// If there's some kind of import profile, we can transform to the
		// output.
		if (image.Contains("icc-profile-data"))
		{
			// Fallback to sRGB.
			inputProfile = "srgb";

			// Convert to sRGB using embedded or import profile.
			outputProfile = "srgb";

			// Use "perceptual" intent to better match *magick.
			intent = Enums.Intent.Perceptual;
		}
		
		foreach (var size in sizes)
		{
			var (width, height) = SizeLogic.ScaledSize(image.Width, image.Height, size);
			// var horizontalShrinkFactor = image.Width / (double)width;
			// var verticalShrinkFactor = image.Height / (double)height;
			//using var resized = image.Reduce(horizontalShrinkFactor, verticalShrinkFactor, kernel: Enums.Kernel.Lanczos3);
			
			// double scale = image.Width / (double)size;
			// var kernel = scale < 1 ? Enums.Kernel.Lanczos3 : Enums.Kernel.Mitchell; // Mitchell is best for enlarging, and shrinking pictures with sharp lines. Otherwise use Lanczos for shrinking.
			// using var resized = image.Resize(scale, kernel);
			
			// thumbnail takes care of color management, the best resampling mechanism etc. https://www.libvips.org/API/current/libvips-resample.html
			
			
			using var resized = Image.Thumbnail(sourcePath, width, height, inputProfile: inputProfile, outputProfile: outputProfile, intent: intent);
			
            var fileName = FileNameLogic.OutputPath(sourcePath, destinationPath, $"NetVips-{size}");

			using var imageNoExif = resized.Mutate(mut =>
				{
					foreach (var field in resized.GetFields())
					{
						// TODO: before I needed the icc data, now I do not anymore? => in Mozilla, with ICC, the colors are off!
						// if (field == "icc-profile-data")
						// 	continue;
						mut.Remove(field);
					}
				}
			);

			using var copy = imageNoExif.CopyMemory();
			
			// save as png
			var pngFileName = $"{fileName}.png";
			copy.Pngsave(pngFileName, quality);
			//copy.WriteToFile(pngFileName);
			
			
			// save as lossless webp
			var webpFileName = $"{fileName}.webp";
			// var webpOptions = new VOption
			// {
			//     { "Q", quality }
			// };
			//copy.WriteToFile(webpFileName, webpOptions);
			// default = 75, default effort = 4 (method)
			copy.Webpsave(webpFileName, q: quality);
			
			// save as lossless webp
			var webpFileLosslessName = $"{fileName}-lossless.webp";
            // var webpOptions = new VOption
            // {
            //     { "Q", quality }
            // };
            //copy.WriteToFile(webpFileName, webpOptions);
            // default = 75, default effort = 4 (method)
            copy.Webpsave(webpFileLosslessName, q: quality, lossless: true, effort:6);
			
            // save as lossy webp
            var webpLossyFileName = $"{fileName}-lossy.webp";
            // var webpOptions = new VOption
            // {
            //     { "Q", quality }
            // };
            //copy.WriteToFile(webpFileName, webpOptions);
            copy.Webpsave(webpLossyFileName, q: 80, lossless: false, smartSubsample: true);
            
			// save as jpg
			var jpgFileName = $"{fileName}.jpg";
			// var kwargs = new VOption
			// {
			// 	{"Q", quality}
			// };
			copy.Jpegsave(jpgFileName, q: quality, subsampleMode: Enums.ForeignSubsample.On);
			//copy.WriteToFile(jpgFileName, kwargs);
		}
	}
}
