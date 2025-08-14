namespace Wemcy.RecipeApp.Backend.Model
{
    public abstract class Entity
    {
        public required Guid Id { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required DateTime UpdatedAt { get; set; }

    }
}
