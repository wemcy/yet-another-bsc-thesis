using Wemcy.RecipeApp.Backend.Exceptions;

namespace Wemcy.RecipeApp.Backend.Services;

public class ImageStorageService
{
    public async Task SaveImage(Guid id, Stream imageData)
    {
        using var filestream = File.OpenWrite(GetImagePath(id));
        await imageData.CopyToAsync(filestream);
    }

    public Stream LoadImage(Guid id)
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
        return Path.Combine("/storage/recipe_images", $"{id}");
    }
}
