namespace Wemcy.RecipeApp.Backend.Model.Entities;

public class Image : Entity
{
    public required string Name { get; set; }

    public required string Extenstion { get; set; }

    public required byte[] HashCode { get; set; }
}
