using SixLabors.ImageSharp;

namespace Wemcy.RecipeApp.Backend.Services;

public interface IImageStorageService
{
    Stream ReadImage(Guid imageId);
    ValueTask<Stream> ReadImage(Guid imageId, Size size);
    Task SaveImage(Guid imageId, Stream imageData);
    Task DeleteImage(Guid imageId);
    ValueTask<byte[]> GetImageHashAsync(Guid id);
}
