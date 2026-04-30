using Wemcy.RecipeApp.Backend.Api.Models;
using Wemcy.RecipeApp.Backend.Services;

namespace Wemcy.RecipeApp.Backend.Utils;

public class ImageResult(IImageService imageService, Guid imageId)
{
    private IImageService _imageService = imageService;
    private Guid _imageId = imageId;

    public required ImageSize ImageSize { get; init; }
    public required string ImageHash { get; init; }
    public async Task<Stream> OpenImageStream()
    {
        return await _imageService.GetImageById(_imageId, ImageSize);
    }

}
