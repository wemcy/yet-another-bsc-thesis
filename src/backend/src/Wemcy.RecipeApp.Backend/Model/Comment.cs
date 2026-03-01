namespace Wemcy.RecipeApp.Backend.Model
{
    public class Comment: Entity
    {
        public virtual Recipe Recipe { get; set; }
        public required string Text { get; set; }
        //TODO USER
    }
}
