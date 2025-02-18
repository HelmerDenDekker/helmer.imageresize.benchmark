using System.Drawing;
using Helmer.ImageResize.Benchmark.Application.Extensions;
using PhotoSauce.MagicScaler;
using PhotoSauce.NativeCodecs.Libwebp;

namespace Helmer.ImageResize.Benchmark.Application.ImageFormats;

public class ResizeMagicScaler
{
    public void ImageResize(int[] sizes, string sourcePath, string destinationPath, int quality)
    {
		CodecManager.Configure(codecs => {
			codecs.UseLibwebp();
		});
        var image = Image.FromStream(File.OpenRead(sourcePath), false, false);

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
				EncoderOptions = new JpegEncoderOptions(quality, ChromaSubsampleMode.Subsample420, true)
			};
			MagicImageProcessor.ProcessImage(sourcePath, jpegOutput, jpgSettings);
			
			using var pngOutput = new FileStream($"{fileName}.png", FileMode.Create);
			var pngSettings = new ProcessImageSettings()
			{
				Width = scaled.width,
				Height = scaled.height,
				ResizeMode = CropScaleMode.Max,
				EncoderOptions = new PngEncoderOptions()
			};
			MagicImageProcessor.ProcessImage(sourcePath, pngOutput, pngSettings);
			
			// using var webpOutput = new FileStream($"{fileName}.webp", FileMode.Create);
			// var webpSettings = new ProcessImageSettings()
			// {
			// 	Width = scaled.width,
			// 	Height = scaled.height,
			// 	ResizeMode = CropScaleMode.Max
			// };
			// webpSettings.TrySetEncoderFormat(ImageMimeTypes.Webp);
			// MagicImageProcessor.ProcessImage(sourcePath, webpOutput, webpSettings);
		}
	}
}