using System.Drawing;
using Helmer.ImageResize.Benchmark.Application.Extensions;
using ImageFlow = Imageflow.Fluent;

namespace Helmer.ImageResize.Benchmark.Application.ImageResize;

/// <summary>
///     Uses Image Flow nuget package https://github.com/imazen/imageflow
/// </summary>
public class ResizeImageFlow
{
	public async Task ImageResize(int[] sizes, string sourcePath, string destinationPath, int quality)
	{
        var original = Image.FromStream(File.OpenRead(sourcePath), false, false);
        
        foreach (var size in sizes)
		{
			var scaled = SizeLogic.ScaledSize(original.Width, original.Height, size);

			var fileName = FileNameLogic.OutputPath(sourcePath, destinationPath, $"Imageflow-{size}");

			using (var jpegOutput = File.Open($"{fileName}.jpg", FileMode.Create))
			{
                var sourceBytes = await File.ReadAllBytesAsync(sourcePath);
                
                using (var image = new ImageFlow.ImageJob())
                {
                    var resized =
                    await image
                        .Decode(sourceBytes)
                        .ResizerCommands($"width={scaled.width}&height={scaled.height}&mode=max")
                        .EncodeToBytes(new ImageFlow.MozJpegEncoder(quality, true))
                        .Finish()
                        .InProcessAsync();

                    if (resized.First.TryGetBytes().HasValue)
                    {
                        var b = resized.First.TryGetBytes().Value.ToArray();
                        await jpegOutput.WriteAsync(b, 0, b.Length);
                    }
                };
            };
        }

	}
}
