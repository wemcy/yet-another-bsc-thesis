namespace Wemcy.RecipeApp.Backend.Model;

public abstract class Entity
{
    [Key]
    public Guid Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

}
