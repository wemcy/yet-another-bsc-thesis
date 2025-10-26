using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wemcy.RecipeApp.Backend.Model;

namespace Wemcy.RecipeApp.Backend.Database;

public class DatabaseContext : DbContext
{
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Allergen> Allergens { get; set; }

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
        modelBuilder.Entity<Allergen>(entity =>
        {
            entity.HasData(
            [
                new() { Type = AllergenType.Gluten },
                new() { Type = AllergenType.Crustaceans },
                new() { Type = AllergenType.Eggs },
                new Allergen { Type = AllergenType.Fish },
                new Allergen { Type = AllergenType.Peanuts },
                new Allergen { Type = AllergenType.Soybeans },
                new Allergen { Type = AllergenType.Milk },
                new Allergen { Type = AllergenType.Nuts },
                new Allergen { Type = AllergenType.Celery },
                new Allergen { Type = AllergenType.Mustard },
                new Allergen { Type = AllergenType.SesameSeeds },
                new Allergen { Type = AllergenType.SulphurDioxide },
                new Allergen { Type = AllergenType.Lupin },
                new Allergen { Type = AllergenType.Molluscs }
            ]);
        });
        modelBuilder.Entity<Allergen>().HasMany(x => x.Recipes).WithMany(x => x.Allergens);
    }

    //To not be able call this outside without context
    protected DatabaseContext()
    {
    }
}
