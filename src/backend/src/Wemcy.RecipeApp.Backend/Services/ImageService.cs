using SixLabors.ImageSharp;
using Wemcy.RecipeApp.Backend.Api.Models;
using Wemcy.RecipeApp.Backend.Repository;

namespace Wemcy.RecipeApp.Backend.Services;

public class ImageService(IImageStorageService imageStorageService, IImageRepository imageRepository) : IImageService
{
    private readonly IImageRepository imageRepository = imageRepository;
    private readonly IImageStorageService imageStorageService = imageStorageService;
    public async Task<Model.Entities.Image> CreateImage(Stream imageStream, string name)
    {
        var image = new Model.Entities.Image
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
        return imageStorageService.ReadImage(id);
    }

    public async ValueTask<Stream> GetImageById(Guid id, Size size)
    {
        return await imageStorageService.ReadImage(id, size);
    }

    public async ValueTask<Stream> GetImageById(Guid id, ImageSize size)
    {
        return await imageStorageService.ReadImage(id, GetImageSize(size));
    }

    private static Size GetImageSize(ImageSize imageSize)
    {
        return imageSize switch
        {
            ImageSize.ThumbnailEnum => new Size(50, 50),
            ImageSize.MediumEnum => new Size(200, 200),
            ImageSize.LargeEnum => new Size(500, 500),
            _ => throw new ArgumentOutOfRangeException(nameof(imageSize), "Invalid image size.")
        };
    }
}
