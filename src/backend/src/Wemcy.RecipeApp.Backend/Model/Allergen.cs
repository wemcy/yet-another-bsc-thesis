namespace Wemcy.RecipeApp.Backend.Model;

[Flags]
public enum AllergenType
{
    None = 0,
    Gluten = 1 << 1,
    Crustaceans = 1 << 2,
    Eggs = 1 << 3,
    Fish = 1 << 4,
    Peanuts = 1 << 5,
    Soybeans = 1 << 6,
    Milk = 1 << 7,
    Nuts = 1 << 8,
    Celery = 1 << 9,
    Mustard = 1 << 10,
    SesameSeeds = 1 << 11,
    SulphurDioxide = 1 << 12,
    Lupin = 1 << 13,
    Molluscs = 1 << 14
}