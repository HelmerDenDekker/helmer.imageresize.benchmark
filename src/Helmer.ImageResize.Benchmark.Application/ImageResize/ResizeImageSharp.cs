using Helmer.ImageResize.Domain.Extensions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;



namespace Helmer.ImageResize.Domain;

// https://docs.sixlabors.com/articles/imagesharp/resize.html
public class ResizeImageSharp
{
	public void ImageResize(int size, string sourcePath, string destinationPath, int quality)
	{

		var imageSharpJpegEncoder = new JpegEncoder() { Quality = quality, ColorType = JpegEncodingColor.YCbCrRatio420 };
		using (var output = File.Open(FileNameLogic.OutputPath(sourcePath, destinationPath, "ImageSharp"), FileMode.Create))
		{
			
			using (var image = Image.Load(sourcePath))
			{
				var scaled = SizeLogic.ScaledSize(image.Width, image.Height, size);
				image.Mutate(i => i.Resize(scaled.width, scaled.height));

				// Reduce the size of the file //ToDo is this cheating??
				image.Metadata.ExifProfile = null;
				image.Metadata.IptcProfile = null;
				image.Metadata.XmpProfile = null;

				// Save the results.
				image.Save(output, imageSharpJpegEncoder);
			}
		}
	}
}