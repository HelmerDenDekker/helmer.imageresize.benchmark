using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Helmer.ImageResize.Benchmark.Application.Extensions;
using SkiaSharp.Views.Desktop;

namespace Helmer.ImageResize.Benchmark.Application.ImageResize;

//ToDo let it inherit from some interface maybe????
/// <summary>
/// Uses the system.drawing of Windows, old Win32 GDI https://www.hanselman.com/blog/how-do-you-use-systemdrawing-in-net-core
/// </summary>
public class ResizeDrawing
{
    // Filepath for saving pictures from settings //ToDo think about width and height
    public void ImageResize(int[] sizes, string sourcePath, string destinationPath, int quality)
    {
        var systemDrawingJpegCodec = ImageCodecInfo.GetImageEncoders().First(codec => codec.FormatID == ImageFormat.Jpeg.Guid);

		using var image = Image.FromFile(sourcePath, true);

		foreach (var size in sizes)
		{
			var scaled = SizeLogic.ScaledSize(image.Width, image.Height, size);
			var resized = new Bitmap(scaled.width, scaled.height);
			
			using var graphics = Graphics.FromImage(resized);

			using var attributes = new ImageAttributes();
			
			attributes.SetWrapMode(WrapMode.TileFlipXY);
			graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
			graphics.CompositingMode = CompositingMode.SourceCopy;
			graphics.CompositingQuality = CompositingQuality.AssumeLinear;
			graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			graphics.DrawImage(image, Rectangle.FromLTRB(0, 0, resized.Width, resized.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);

			// Save the results
			using var encoderParams = new EncoderParameters(1);
			using var qualityParam = new EncoderParameter(Encoder.Quality, quality);

			encoderParams.Param[0] = qualityParam;

			var fileName = FileNameLogic.OutputPath(sourcePath, destinationPath, $"SystemDrawing-{size}");

			ImageFormat webp = new ImageFormat(new System.Guid("{B73C6BB9-2807-D311-9D7B-0000F81EF32E}"));
            //resized.Save($"{fileName}.webp", webp);
            resized.Save($"{fileName}.png", ImageFormat.Png);
			resized.Save($"{fileName}.jpg", systemDrawingJpegCodec, encoderParams);

			using var skImage = resized.ToSKImage();
			using var skData = skImage.Encode(SkiaSharp.SKEncodedImageFormat.Webp, quality);
			using var file = File.OpenWrite($"{fileName}-SK.webp");
			skData.SaveTo(file);
		}
	}
}