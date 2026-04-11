using SixLabors.ImageSharp;
using Wemcy.RecipeApp.Backend.Api.Models;

namespace Wemcy.RecipeApp.Backend.Services;

public interface IImageService
{
    Task<Model.Entities.Image> CreateImage(Stream imageStream, string name);
    Stream GetImageById(Guid id);
    ValueTask<Stream> GetImageById(Guid id, ImageSize size);
    ValueTask<Stream> GetImageById(Guid id, Size size);
}
