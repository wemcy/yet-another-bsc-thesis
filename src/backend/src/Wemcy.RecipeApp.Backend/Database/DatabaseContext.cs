using Microsoft.EntityFrameworkCore;
using Wemcy.RecipeApp.Backend.Model;

namespace Wemcy.RecipeApp.Backend.Database;

public class DatabaseContext : DbContext
{
    public DbSet<Recipe> Recipes { get; set; }  
    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    //To not be able call this outside without context
    protected DatabaseContext()
    {
    }
}
