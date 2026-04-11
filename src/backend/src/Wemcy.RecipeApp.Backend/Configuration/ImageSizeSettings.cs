namespace Wemcy.RecipeApp.Backend.Configuration;

public class ImageSizeSettings
{
    public const string SectionName = "ImageSizes";

    public SizeOption Thumbnail { get; set; } = new(50, 50);
    public SizeOption Medium { get; set; } = new(200, 200);
    public SizeOption Large { get; set; } = new(500, 500);
}

public class SizeOption
{
    public int Width { get; set; }
    public int Height { get; set; }

    public SizeOption() { }
    public SizeOption(int width, int height) { Width = width; Height = height; }
}
