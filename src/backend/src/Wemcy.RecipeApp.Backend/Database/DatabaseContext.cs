using Microsoft.EntityFrameworkCore;
using Wemcy.RecipeApp.Backend.Model;

namespace Wemcy.RecipeApp.Backend.Database;

public class DatabaseContext : DbContext
{
    public DbSet<Recipe> Recipes { get; set; }  
    public DatabaseContext(DbContextOptions options) : base(options)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Recipe>(entity =>
        {

            entity.HasKey(r => r.Id);
            entity.Property(r => r.Id)
                  .ValueGeneratedOnAdd();
        });
    }

    //To not be able call this outside without context
    protected DatabaseContext()
    {
    }
}
