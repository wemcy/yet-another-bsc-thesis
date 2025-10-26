using System.ComponentModel.DataAnnotations;

namespace Wemcy.RecipeApp.Backend.Model
{
    public class Allergen
    {
        [Key]
        public required AllergenType Type { get; set; }
        public virtual IList<Recipe> Recipes { get; set; } = [];
    }
    public enum AllergenType
    {
        Gluten = 1,
        Crustaceans = 2,        // Rákfélék
        Eggs = 3,
        Fish = 4,
        Peanuts = 5,
        Soybeans = 6,
        Milk = 7,
        Nuts = 8,               // Diófélék
        Celery = 9,
        Mustard = 10,
        SesameSeeds = 11,
        SulphurDioxide = 12,
        Lupin = 13,
        Molluscs = 14
    }
}