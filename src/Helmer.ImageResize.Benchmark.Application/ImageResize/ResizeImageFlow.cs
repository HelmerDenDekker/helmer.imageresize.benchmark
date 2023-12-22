using System.Drawing;
using Helmer.ImageResize.Benchmark.Application.Extensions;
using ImageFlow = Imageflow.Fluent;

namespace Helmer.ImageResize.Benchmark.Application.ImageResize;


/// <summary>
/// Uses Image Flow nuget package https://github.com/imazen/imageflow
/// </summary>
public class ResizeImageFlow
{
    // Filepath for saving pictures from settings //ToDo think about width and height
    public async Task ImageResize(int size, string sourcePath, string destinationPath, int quality)
    {
		using (var output = File.Open(FileNameLogic.OutputPath(sourcePath, destinationPath, "Imageflow"), FileMode.Create))
		{
			var imageBytes = await File.ReadAllBytesAsync(sourcePath);

			var original = Image.FromStream(File.OpenRead(sourcePath), false, false);
			 
			var scaled = SizeLogic.ScaledSize(original.Width, original.Height, size);
			using (var image = new ImageFlow.ImageJob())
			{
				
                var o =
					await image
						.Decode(imageBytes)
						.ResizerCommands($"width={scaled.width}&height={scaled.height}&mode=max")
						.EncodeToBytes(new ImageFlow.MozJpegEncoder(quality, true))
						.Finish()
						.InProcessAsync();

				// Don't throw, bad for benchmark.
				if (o.First.TryGetBytes().HasValue)
				{
					var b = o.First.TryGetBytes().Value.ToArray();
					await output.WriteAsync(b, 0, b.Length);
				}
			}
		}
    }


}