using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Wemcy.RecipeApp.Backend.Model.Entities;
using Wemcy.RecipeApp.Backend.Utils;


namespace Wemcy.RecipeApp.Backend.Database;

public class DatabaseContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Image> Images { get; set; }

    public DbSet<IngredientSuggestion> IngredientSuggestions { get; set; }

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
            entity.Property(r => r.Allergens)
                  .HasConversion<int>();
            entity.HasGeneratedTsVectorColumn(x => x.TitleSearchVector, "english", x => x.Title).HasIndex(x => x.TitleSearchVector).HasMethod("GIN");
            entity.HasData(DefaultRecipies.GetDefaultRecipes());
        });
        modelBuilder.Entity<RecipeShowcase>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Id)
                  .ValueGeneratedNever();
            entity.Property(r => r.ShowcaseRecipeIds)
                  .HasColumnType("uuid[]");
            entity.ToTable(t => t.HasCheckConstraint("CK_RecipeShowcase_Singleton", "\"Id\" = 1"));
            entity.HasData(new RecipeShowcase { Id = RecipeShowcase.SingletonId });
        });
        modelBuilder.Entity<IngredientSuggestion>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.Id)
                  .ValueGeneratedOnAdd();
            entity.Property(r => r.Allergens)
                  .HasConversion<int>();
            entity.HasGeneratedTsVectorColumn(x => x.NameSearchVector, "english", x => x.Name).HasIndex(x => x.NameSearchVector).HasMethod("GIN");
            entity.HasData(DefaultRecipies.GetDefaultIngredientSuggestions());

        });
    }



    //To not be able call this outside without context
    protected DatabaseContext()
    {
    }

    public override int SaveChanges()
    {
        UpdateEntityTimestamps();
        UpdateUserTimestamps();
        UpdateEntities();
        return base.SaveChanges();
    }

    private void UpdateEntities()
    {
        var entries = ChangeTracker.Entries<Recipe>().ToList();

        foreach (var entry in entries)
        {

            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.State == EntityState.Unchanged)
            {
                entry.Entity.OnSave();
            }
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateEntityTimestamps();
        UpdateUserTimestamps();
        UpdateEntities();

        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateEntityTimestamps()
    {
        var entries = ChangeTracker.Entries<Entity>().ToList();
        var now = DateTimeOffset.UtcNow;

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
                entry.Entity.UpdatedAt = now;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
            }
        }
    }

    private void UpdateUserTimestamps()
    {
        var entries = ChangeTracker.Entries<User>().ToList();
        var now = DateTimeOffset.UtcNow;
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.RegisteredAt = now;
            }
        }
    }
}
