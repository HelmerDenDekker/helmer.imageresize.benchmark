namespace Helmer.ImageResize.Benchmark.Application.Extensions;

public static class FileNameLogic
{
	public static string OutputPath(string inputPath, string outputDirectory, string postfix)
	{
		return Path.Combine(
			outputDirectory,
			Path.GetFileNameWithoutExtension(inputPath)
			+ "-" + postfix
			+ Path.GetExtension(inputPath));
	}
}