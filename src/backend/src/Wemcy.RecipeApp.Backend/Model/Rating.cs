namespace Wemcy.RecipeApp.Backend.Model
{
    public class Rating : Entity
    {
        public virtual Recipe Recipe { get; set; } = null!; // Entity framwork, we need to set this to null, but it will be set when we add the rating to the recipe
        public required int Value { get; set; }
        //TODO USER
    }
}
