using Wemcy.RecipeApp.Backend.Database;
using Wemcy.RecipeApp.Backend.Model;

namespace Wemcy.RecipeApp.Backend.Repository;

public class ImageRepository(DatabaseContext databaseContext)
{
    private readonly DatabaseContext _dbContext = databaseContext;

    public Image SaveImage(Image image)
    {
        _dbContext.Images.Add(image);
        _dbContext.SaveChanges();
        return image;
    }

}
