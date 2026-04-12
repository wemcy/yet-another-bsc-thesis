using Wemcy.RecipeApp.Backend.Model.Entities;

namespace Wemcy.RecipeApp.Backend.Repository;

public interface IImageRepository
{
    Image SaveImage(Image image);
}
