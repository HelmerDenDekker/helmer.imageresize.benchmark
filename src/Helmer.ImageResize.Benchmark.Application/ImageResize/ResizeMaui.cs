﻿
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using Helmer.ImageResize.Domain.Extensions;
using Microsoft.Maui.Graphics;
#if IOS || ANDROID || MACCATALYST
using Microsoft.Maui.Graphics.Platform;
#elif WINDOWS
using Microsoft.Maui.Graphics.Win2D;
#endif


namespace Helmer.ImageResize.Domain.Resize
{
    public class ResizeMaui
    {

        public void ImageResize(int size, string sourcePath, string destinationPath, int quality)
        {
			
            IImage image;

            using (var stream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
            {
#if IOS || ANDROID
                image = PlatformImage.FromStream(stream, ImageFormat.Jpeg);

#elif WINDOWS
        image = new W2DImageLoadingService().FromStream(stream);
#endif
            }

            if (image != null)
            {
                var scaled = SizeLogic.ScaledSize(image.Width, image.Height, size);
                IImage newImage = image.Resize(scaled.width, scaled.height, ResizeMode.Stretch);
                using (MemoryStream memStream = new MemoryStream())
                {
                    newImage.Save(memStream, ImageFormat.Jpeg, quality);
					using (FileStream output = File.Create(FileNameLogic.OutputPath(sourcePath, destinationPath, "Maui")))
					{
						memStream.CopyTo(output);
					}
                }
            }
        }
    }
}

