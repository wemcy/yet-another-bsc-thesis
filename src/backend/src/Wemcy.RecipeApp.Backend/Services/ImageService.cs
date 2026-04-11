using Wemcy.RecipeApp.Backend.Model.Entities;
using Wemcy.RecipeApp.Backend.Repository;

namespace Wemcy.RecipeApp.Backend.Services;

public class ImageService(ImageStorageService imageStorageService, ImageRepository imageRepository)
{
    // TODO fix images extensions thingys
    private readonly ImageRepository imageRepository = imageRepository;
    private readonly ImageStorageService imageStorageService = imageStorageService;
    public async Task<Image> CreateImage(Stream imageStream, string name)
    {
        var image = new Image
        {
            Id = Guid.NewGuid(),
            Name = name,
            Extenstion = Path.GetExtension(name)
        };
        await imageStorageService.SaveImage(image.Id, imageStream);
        return imageRepository.SaveImage(image);
    }
    public Stream GetImageById(Guid id)
    {
        return imageStorageService.LoadImage(id);
    }
}
