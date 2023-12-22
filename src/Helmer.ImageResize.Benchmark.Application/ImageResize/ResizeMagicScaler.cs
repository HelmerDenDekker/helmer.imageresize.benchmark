using System.Drawing;
using Helmer.ImageResize.Domain.Extensions;
using PhotoSauce.MagicScaler;

namespace Helmer.ImageResize.Domain;

public class ResizeMagicScaler
{
	public void ImageResize(int size, string sourcePath, string destinationPath, int quality)
	{
		var image = Image.FromStream(File.OpenRead(sourcePath), false, false);

        var scaled = SizeLogic.ScaledSize(image.Width, image.Height, size);
        var settings = new ProcessImageSettings()
		{
			Width = scaled.width,
			Height = scaled.height,
			ResizeMode = CropScaleMode.Max,
			EncoderOptions = new JpegEncoderOptions(quality, ChromaSubsampleMode.Subsample420, true)
        };

		using (var output = new FileStream(FileNameLogic.OutputPath(sourcePath, destinationPath, "MagicScaler"), FileMode.Create))
		{
			MagicImageProcessor.ProcessImage(sourcePath, output, settings);
		}
	}
}