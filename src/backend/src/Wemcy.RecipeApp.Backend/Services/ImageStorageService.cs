using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Wemcy.RecipeApp.Backend.Exceptions;

namespace Wemcy.RecipeApp.Backend.Services;

public class ImageStorageService : IImageStorageService
{
    private const string directory = "/storage/recipe_images";
    public async Task SaveImage(Guid id, Stream imageData)
    {
        using var image = await Image.LoadAsync(imageData);
        await image.SaveAsJpegAsync(GetImagePath(id));
    }

    public Stream ReadImage(Guid id)
    {
        try
        {
            var filestream = File.OpenRead(GetImagePath(id));
            return filestream;
        }
        catch (FileNotFoundException)
        {
            throw new ImageNotFoundException(); //TODO : Add image ID to exception message, and other details if needed
        }
    }

    private static string GetImagePath(Guid id)
    {
        return Path.Combine(directory, $"{id}");
    }

    private static string GetImagePath(Guid id, Size size)
    {
        return Path.Combine(directory, $"{id}_{size.Width}x{size.Height}");
    }

    public async ValueTask<Stream> ReadImage(Guid imageId, Size size)
    {
        var path = GetImagePath(imageId, size);
        if (File.Exists(path)) return File.OpenRead(path);
        await Resize(imageId, size);
        return File.OpenRead(path);
    }

    public Task DeleteImage(Guid imageId)
    {
        foreach (var file in GetAllImageWithId(imageId))
            File.Delete(file);
        return Task.CompletedTask;
    }

    private async Task Resize(Guid imageId, Size size)
    {
        using var imageStream = ReadImage(imageId);
        using var image = await Image.LoadAsync(imageStream);
        image.Mutate(x => x.Resize(size));
        var resizedImagePath = GetImagePath(imageId, size);
        await image.SaveAsJpegAsync(resizedImagePath);
    }

    private static string[] GetAllImageWithId(Guid imageId)
    {
        return Directory.GetFiles(directory, $"{imageId}*");
    }
}
