using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using System.Security.Cryptography;
using Wemcy.RecipeApp.Backend.Api.Models;
using Wemcy.RecipeApp.Backend.Configuration;
using Wemcy.RecipeApp.Backend.Repository;

namespace Wemcy.RecipeApp.Backend.Services;

public class ImageService(IImageStorageService imageStorageService, IImageRepository imageRepository, IOptions<ImageSizeSettings> imageSizeSettings) : IImageService
{
    private readonly IImageRepository imageRepository = imageRepository;
    private readonly IImageStorageService imageStorageService = imageStorageService;
    private readonly ImageSizeSettings _imageSizeSettings = imageSizeSettings.Value;
    public async Task<Model.Entities.Image> CreateImage(Stream imageStream, string name)
    {
        var imageId = Guid.NewGuid();
        await imageStorageService.SaveImage(imageId, imageStream);
        var hashCode = await imageStorageService.GetImageHashAsync(imageId);
        var image = new Model.Entities.Image
        {
            Id = imageId,
            Name = name,
            Extenstion = Path.GetExtension(name),
            HashCode = hashCode
        };
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

    private Size GetImageSize(ImageSize imageSize)
    {
        var option = imageSize switch
        {
            ImageSize.ThumbnailEnum => _imageSizeSettings.Thumbnail,
            ImageSize.MediumEnum => _imageSizeSettings.Medium,
            ImageSize.LargeEnum => _imageSizeSettings.Large,
            _ => throw new ArgumentOutOfRangeException(nameof(imageSize), "Invalid image size.")
        };
        return new Size(option.Width, option.Height);
    }
}
