using Helmer.ImageResize.Benchmark.Application.Extensions;
using Microsoft.Maui.Graphics;
using System.Runtime.InteropServices;
using Microsoft.Maui.Graphics.Platform;
//using Microsoft.Maui.Graphics.Win2D;


namespace Helmer.ImageResize.Benchmark.Application.ImageResize
{
    public class ResizeMaui
    {

        public void ImageResize(int size, string sourcePath, string destinationPath, int quality)
		{
			var image = Image(sourcePath);

			if (image != null)
            {
                var scaled = SizeLogic.ScaledSize(image.Width, image.Height, size);
                IImage newImage = image.Resize(scaled.width, scaled.height, ResizeMode.Stretch);
                using (MemoryStream memStream = new MemoryStream())
                {
                    newImage.Save(memStream, ImageFormat.Jpeg, quality);
					using (FileStream output = File.Create(FileNameLogic.OutputPath(sourcePath, destinationPath, "Maui")))
					{
						memStream.Seek(0, SeekOrigin.Begin);
                        memStream.CopyTo(output);
					}
                }
            }
		}

		private static IImage Image(string sourcePath)
		{
			IImage image;

			using (var stream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
			{
				
				if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
					return PlatformImage.FromStream(stream, ImageFormat.Jpeg);
				// Win2D only for windows10
				throw new NotImplementedException();
				//image = new W2DImageLoadingService().FromStream(stream);
			}

			return image;
		}
	}
}

