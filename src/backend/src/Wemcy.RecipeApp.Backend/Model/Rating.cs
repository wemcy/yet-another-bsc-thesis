namespace Wemcy.RecipeApp.Backend.Model
{
    public class Rating : Entity
    {
        public virtual Recipe Recipe { get; set; }
        public required int Value { get; set; }
        //TODO USER
    }
}
