using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Helmer.ImageResize.Benchmark.Application.Extensions;

namespace Helmer.ImageResize.Benchmark.Application.ImageResize;

/// <summary>
///     Uses the system.drawing of Windows, old Win32 GDI
///     https://www.hanselman.com/blog/how-do-you-use-systemdrawing-in-net-core
/// </summary>
public class ResizeDrawing
{
	public void ImageResize(int[] sizes, string sourcePath, string destinationPath, int quality)
	{
		var systemDrawingJpegCodec = ImageCodecInfo.GetImageEncoders().First(codec => codec.FormatID == ImageFormat.Jpeg.Guid);

		using var image = Image.FromFile(sourcePath, true);

		foreach (var size in sizes)
		{
			var scaled = SizeLogic.ScaledSize(image.Width, image.Height, size);
			using var resized = new Bitmap(scaled.width, scaled.height);

			using var graphics = Graphics.FromImage(resized);

			using var attributes = new ImageAttributes();

			attributes.SetWrapMode(WrapMode.TileFlipXY);
			graphics.PixelOffsetMode = PixelOffsetMode.HighQuality; // Highest quality
			graphics.CompositingMode = CompositingMode.SourceCopy;
			graphics.CompositingQuality = CompositingQuality.HighQuality; // Highest quality https://learn.microsoft.com/en-us/dotnet/api/system.drawing.drawing2d.compositingquality?view=windowsdesktop-9.0&viewFallbackFrom=net-8.0
			graphics.InterpolationMode = InterpolationMode.HighQualityBicubic; // Highest quality
			graphics.SmoothingMode = SmoothingMode.HighQuality; // Highest quality
			graphics.DrawImage(image, Rectangle.FromLTRB(0, 0, resized.Width, resized.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);

			// Save the results
			using var encoderParams = new EncoderParameters(1);
			using var qualityParam = new EncoderParameter(Encoder.Quality, quality);

			encoderParams.Param[0] = qualityParam;

			var fileName = FileNameLogic.OutputPath(sourcePath, destinationPath, $"SystemDrawing-{size}");

			resized.Save($"{fileName}.jpg", systemDrawingJpegCodec, encoderParams);
		}
	}
}
