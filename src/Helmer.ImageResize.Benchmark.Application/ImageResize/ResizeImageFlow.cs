using Helmer.ImageResize.Benchmark.Application.Extensions;
using Imageflow.Fluent;

namespace Helmer.ImageResize.Benchmark.Application.ImageResize;

/// <summary>
///     Uses Image Flow nuget package https://github.com/imazen/imageflow
/// </summary>
public class ResizeImageFlow
{
    public async Task ImageResize(int[] sizes, string sourcePath, string destinationPath, int quality)
    {
        // using var stream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read);
        // var streamSource = BufferedStreamSource.UseEntireStreamAndDisposeWithSource(stream);
        var src = FileSource.FromPath(sourcePath);

        var info = await ImageJob.GetImageInfoAsync(src, SourceLifetime.NowOwnedAndDisposedByTask);
        
        foreach (var size in sizes)
        {
            var scaled = SizeLogic.ScaledSize(info.ImageWidth, info.ImageHeight, size);

            var fileName = FileNameLogic.OutputPath(sourcePath, destinationPath, $"Imageflow-{size}.jpg");

            using (var image = new ImageJob())
            {
                await image
                    .DecodeFile(sourcePath)
                    .ResizerCommands($"width={scaled.width}&height={scaled.height}&mode=max")
                    .Encode(FileDestination.ToPath(fileName), new MozJpegEncoder(quality, true))
                    .Finish()
                    .InProcessAsync();
            }
        }
    }
}