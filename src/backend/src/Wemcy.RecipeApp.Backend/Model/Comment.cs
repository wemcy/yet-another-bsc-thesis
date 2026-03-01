namespace Wemcy.RecipeApp.Backend.Model
{
    public class Comment: Entity
    {
        public virtual Recipe Recipe { get; set; }
        public required string Content { get; set; }
        //TODO USER
    }
}
