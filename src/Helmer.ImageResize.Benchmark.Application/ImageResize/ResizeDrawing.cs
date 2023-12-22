using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Helmer.ImageResize.Domain.Extensions;

namespace Helmer.ImageResize.Domain.Resize;

//ToDo let it inherit from some interface maybe????
/// <summary>
/// Uses the system.drawing of Windows, old Win32 GDI https://www.hanselman.com/blog/how-do-you-use-systemdrawing-in-net-core
/// </summary>
public class ResizeDrawing
{
    // Filepath for saving pictures from settings //ToDo think about width and height
    public void ImageResize(int size, string sourcePath, string destinationPath, int quality)
    {
        var systemDrawingJpegCodec = ImageCodecInfo.GetImageEncoders().First(codec => codec.FormatID == ImageFormat.Jpeg.Guid);

        using (var image = Image.FromFile(sourcePath, true))
        {
            var scaled = SizeLogic.ScaledSize(image.Width, image.Height, size);
            var resized = new Bitmap(scaled.width, scaled.height);
            using (var graphics = Graphics.FromImage(resized))
            using (var attributes = new ImageAttributes())
            {
                attributes.SetWrapMode(WrapMode.TileFlipXY);
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.AssumeLinear;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(image, Rectangle.FromLTRB(0, 0, resized.Width, resized.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);

                // Save the results
                using (var encoderParams = new EncoderParameters(1))
                using (var qualityParam = new EncoderParameter(Encoder.Quality, quality))
                {
                    encoderParams.Param[0] = qualityParam;
                    resized.Save(FileNameLogic.OutputPath(sourcePath, destinationPath, "SystemDrawing"), systemDrawingJpegCodec, encoderParams);
                }
            }
        }
    }
}