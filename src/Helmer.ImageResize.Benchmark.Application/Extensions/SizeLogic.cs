namespace Helmer.ImageResize.Benchmark.Application.Extensions;

public static class SizeLogic
{
	public static (int width, int height) ScaledSize(int inWidth, int inHeight, int outSize)
	{
		int width, height;
		if (inWidth > inHeight)
		{
			width = outSize;
			height = (int)Math.Round(inHeight * outSize / (double)inWidth);
		}
		else
		{
			width = (int)Math.Round(inWidth * outSize / (double)inHeight);
			height = outSize;
		}

		return (width, height);
	}

	public static (int width, int height) ScaledSize(float inWidth, float inHeight, int outSize)
    {
		int width, height;
		if (inWidth > inHeight)
		{
			width = outSize;
			height = (int)Math.Round(inHeight * outSize / (double)inWidth);
		}
		else
		{
			width = (int)Math.Round(inWidth * outSize / (double)inHeight);
			height = outSize;
		}

		return (width, height);
    }
}