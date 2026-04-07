namespace Wemcy.RecipeApp.Backend.Exceptions;

public class CommentNotFoundExeption(Guid commentId) : EntityNotFoundExeption("Comment not found.")
{
    public Guid CommentId { get; } = commentId;
}