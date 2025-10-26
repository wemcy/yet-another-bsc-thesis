using System.ComponentModel.DataAnnotations;

namespace Wemcy.RecipeApp.Backend.Model
{
    public abstract class Entity
    {
        [Key]
        public required Guid Id { get; set; }
        public required DateTimeOffset CreatedAt { get; set; }
        public required DateTimeOffset UpdatedAt { get; set; }

    }
}
