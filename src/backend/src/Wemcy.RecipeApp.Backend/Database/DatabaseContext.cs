using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Wemcy.RecipeApp.Backend.Model;
using Wemcy.RecipeApp.Backend.Utils;


namespace Wemcy.RecipeApp.Backend.Database;

public class DatabaseContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Image> Images { get; set; }

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
    }



    //To not be able call this outside without context
    protected DatabaseContext()
    {
    }

    public override int SaveChanges()
    {
        UpdateEntityTimestamps();
        UpdateUserTimestamps();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateEntityTimestamps();
        UpdateUserTimestamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateEntityTimestamps()
    {
        var entries = ChangeTracker.Entries<Entity>();
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
        var entries = ChangeTracker.Entries<User>();
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
