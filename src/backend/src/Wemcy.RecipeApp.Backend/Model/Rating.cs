using System.ComponentModel.DataAnnotations;

namespace Wemcy.RecipeApp.Backend.Model;

public class Rating : Entity
{
    public virtual Recipe Recipe { get; set; } = null!;
    [Range(1, 5)]
    public required int Value { get; set; }
    public virtual required AppUser User { get; set; } = null!;
}
