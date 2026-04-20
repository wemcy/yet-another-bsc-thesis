namespace Wemcy.RecipeApp.Backend.Model.Entities;

public abstract class Entity
{
    [Key]
    public Guid Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public virtual void OnSave()
    {

    }
}
