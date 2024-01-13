using Helmer.ImageResize.Benchmark.Application.Extensions;
using SkiaSharp;

namespace Helmer.ImageResize.Benchmark.Application.ImageResize;
/// <summary>
/// https://schwabencode.com/blog/2019/06/11/Resize-Image-NET-Core Skia was made to be fast
/// </summary>
public class ResizeSkiaSharp
{
    public void ImageResize(int[] sizes, string sourcePath, string destinationPath, int quality)
    {
        using (var original = SKBitmap.Decode(sourcePath))
        {
			foreach (var size in sizes)
			{
				var scaled = SizeLogic.ScaledSize(original.Width, original.Height, size);
				using (var resized = original.Resize(new SKImageInfo(scaled.width, scaled.height), SKFilterQuality.High))
				{
					if (resized == null)
					{
						return;
					}

					using (var image = SKImage.FromBitmap(resized))
					using (var output = File.OpenWrite(FileNameLogic.OutputPath(sourcePath, destinationPath, "SkiaSharp")))
					{
						image.Encode(SKEncodedImageFormat.Jpeg, quality)
							.SaveTo(output);
					}
				}
			}
		}
    }
}