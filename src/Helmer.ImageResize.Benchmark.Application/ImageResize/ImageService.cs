namespace Helmer.ImageResize.Benchmark.Application.ImageResize;

public class ImageService
{
    private IEnumerable<string> _images;
    private string _imageDirectory;
    private string _outputDirectory;

    public ImageService()
    {
        _imageDirectory = FindClosestDirectory();
        _images = Load(_imageDirectory);
        _outputDirectory = CreateOutput(_imageDirectory);
    }


    public void SystemDrawingBenchmark(int[] sizes, int quality)
    {
        foreach (string image in _images)
        {
            new ResizeDrawing().ImageResize(sizes, image, _outputDirectory, quality);
        }
    }

    public void ImageSharpBenchmark(int[] sizes, int quality)
    {
        foreach (string image in _images)
        {
            new ResizeImageSharp().ImageResize(sizes, image, _outputDirectory, quality);
        }
    }

    public void MagickNetBenchmark(int[] sizes, int quality)
    {
        foreach (string image in _images)
        {
            new ResizeMagickNet().ImageResize(sizes, image, _outputDirectory, quality);
        }
    }
    public void MagicScalerBenchmark(int[] sizes, int quality)
    {
        foreach (string image in _images)
        {
            new ResizeMagicScaler().ImageResize(sizes, image, _outputDirectory, quality);
        }
    }

    public void SkiaSharpBenchmark(int[] sizes, int quality)
    {
        foreach (string image in _images)
        {
            new ResizeSkiaSharp().ImageResize(sizes, image, _outputDirectory, quality);
        }
    }
    //public void FreeImageBenchmark(int[] sizes, int quality)
    //{
    //    foreach (string image in _images)
    //    {
    //        new ResizeFreeImage().ImageResize(size, image, _outputDirectory, quality);
    //    }
    //}

    public void ImageFlowBenchmark(int[] sizes, int quality)
    {
        foreach (string image in _images)
        {
            new ResizeImageFlow().ImageResize(sizes, image, _outputDirectory, quality);
        }
    }

    //public void MauiBenchmark(int[] sizes, int quality)
    //{
    //    foreach (string image in _images)
    //    {
    //        new ResizeMaui().ImageResize(size, image, _outputDirectory, quality);
    //    }
    //}




    /// <summary>
    /// Loads filenames from the images directory //ToDo validation which is now in FindClosestDirectory
    /// </summary>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException"></exception>
    private static IEnumerable<string> Load(string imageDirectory)
    {
        return Directory.EnumerateFiles(imageDirectory);
    }

    /// <summary>
    /// Crete Output directory //ToDo change it to take an input path later
    /// </summary>
    /// <param name="imageDirectory"></param>
	private static string CreateOutput(string imageDirectory)
    {
        // Create the output directory next to the images directory
        var outputDirectory = Path.Combine(Path.GetDirectoryName(imageDirectory), "output");
        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }
        return outputDirectory;
    }

    /// <summary>
    /// Put the files in an images directory and let this function find them
    /// </summary>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException"></exception>
	private static string FindClosestDirectory()
    {
        // Find the closest images directory
        string imageDirectory = Path.GetFullPath(".");
        while (!Directory.Exists(Path.Combine(imageDirectory, "images")))
        {
            imageDirectory = Path.GetDirectoryName(imageDirectory);
            if (imageDirectory == null)
            {
                throw new FileNotFoundException("Could not find an image directory.");
            }
        }

        return Path.Combine(imageDirectory, "images");
    }
}