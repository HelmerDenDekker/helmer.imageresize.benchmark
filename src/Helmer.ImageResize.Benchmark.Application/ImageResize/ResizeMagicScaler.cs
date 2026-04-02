using System.Drawing;
using Helmer.ImageResize.Benchmark.Application.Extensions;
using PhotoSauce.MagicScaler;
using PhotoSauce.NativeCodecs.Libwebp;

namespace Helmer.ImageResize.Benchmark.Application.ImageResize;

public class ResizeMagicScaler
{
    public void ImageResize(int[] sizes, string sourcePath, string destinationPath, int quality)
    {
        using var sourceStream = File.OpenRead(sourcePath);
        using var image = Image.FromStream(sourceStream, false, false);

        foreach (var size in sizes)
		{

			var scaled = SizeLogic.ScaledSize(image.Width, image.Height, size);
			
			var fileName = FileNameLogic.OutputPath(sourcePath, destinationPath, $"MagicScaler-{size}");
			
			using var jpegOutput =  new FileStream($"{fileName}.jpg", FileMode.Create);
			var jpgSettings = new ProcessImageSettings()
			{
				Width = scaled.width,
				Height = scaled.height,
				ResizeMode = CropScaleMode.Max,
				EncoderOptions = new JpegEncoderOptions(quality, ChromaSubsampleMode.Subsample444, true) // 444 means highest quality
			};
			MagicImageProcessor.ProcessImage(sourcePath, jpegOutput, jpgSettings);
			
		}
	}
}