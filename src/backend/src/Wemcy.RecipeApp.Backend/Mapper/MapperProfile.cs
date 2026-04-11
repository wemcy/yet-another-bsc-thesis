using AutoMapper;
using Wemcy.RecipeApp.Backend.Model;
using Allergen = Wemcy.RecipeApp.Backend.Api.Models.Allergen;
using RecipeDTO = Wemcy.RecipeApp.Backend.Api.Models.Recipe;


namespace Wemcy.RecipeApp.Backend.Mapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<RecipeDTO, Recipe>()
            .ForMember(
                x => x.Allergens,
                op => op.MapFrom(src => MapAllergensListToAllergen(src.Allergens)));

        CreateMap<Recipe, RecipeDTO>()
            .ForMember(dest => dest.Allergens, op => op.MapFrom(src => MappAllergenDTO(src.Allergens)));

        CreateMap<Ingredient, Api.Models.Ingredient>()
            .ForMember(dest => dest.Quantity, 
                opt => opt.MapFrom(src => Convert.ToDecimal(src.Quantity)))
            .ReverseMap()
            .ForMember(dest => dest.Quantity, 
                opt => opt.MapFrom(src => Convert.ToDouble(src.Quantity)));

        CreateMap<Comment, Api.Models.Comment>()
            .ForMember(dest => dest.Author, 
                opt => opt.MapFrom(src => src.User.DisplayName));

        CreateMap<AppUser, Api.Models.Profile>();

        CreateMap<Recipe, Api.Models.RecipeSummary>();

        CreateMap<List<Allergen>?, AllergenType?>().ConvertUsing(src => MapAllergensListToAllergen(src));


    }
    private static AllergenType? MapAllergensListToAllergen(IList<Allergen>? src)
    {
        if (src is null) return null;
        return src.Select(MapAllergens).Aggregate(AllergenType.None, (current, allergen) => current | allergen);
    }

    private static IList<Allergen> MappAllergenDTO(AllergenType allergen)
    {
        return [.. Enum.GetValues<AllergenType>()
            .Where( f => f != AllergenType.None)
            .Where(f => allergen.HasFlag(f))
            .Select(MapAllergenTypeToAllergen)];
    }
    private static AllergenType MapAllergens(Allergen allergen)
    {
        return allergen switch
        {
            Allergen.GLUTENEnum => AllergenType.Gluten,
            Allergen.CRUSTACEANSEnum => AllergenType.Crustaceans,
            Allergen.EGGSEnum => AllergenType.Eggs,
            Allergen.FISHEnum => AllergenType.Fish,
            Allergen.PEANUTSEnum => AllergenType.Peanuts,
            Allergen.SOYBEANSEnum => AllergenType.Soybeans,
            Allergen.MILKEnum => AllergenType.Milk,
            Allergen.NUTSEnum => AllergenType.Nuts,
            Allergen.CELERYEnum => AllergenType.Celery,
            Allergen.MUSTARDEnum => AllergenType.Mustard,
            Allergen.SESAMESEEDSEnum => AllergenType.SesameSeeds,
            Allergen.SULPHURDIOXIDEEnum => AllergenType.SulphurDioxide,
            Allergen.LUPINEnum => AllergenType.Lupin,
            Allergen.MOLLUSCSEnum => AllergenType.Molluscs,
            _ => AllergenType.None
        };
    }

    private static Allergen MapAllergenTypeToAllergen( AllergenType allergen)
    {
        return allergen switch
        {
            AllergenType.Gluten => Allergen.GLUTENEnum,
            AllergenType.Crustaceans => Allergen.CRUSTACEANSEnum,
            AllergenType.Eggs => Allergen.EGGSEnum,
            AllergenType.Fish => Allergen.FISHEnum,
            AllergenType.Peanuts => Allergen.PEANUTSEnum,
            AllergenType.Soybeans => Allergen.SOYBEANSEnum,
            AllergenType.Milk => Allergen.MILKEnum,
            AllergenType.Nuts => Allergen.NUTSEnum,
            AllergenType.Celery => Allergen.CELERYEnum,
            AllergenType.Mustard => Allergen.MUSTARDEnum,
            AllergenType.SesameSeeds => Allergen.SESAMESEEDSEnum,
            AllergenType.SulphurDioxide => Allergen.SULPHURDIOXIDEEnum,
            AllergenType.Lupin => Allergen.LUPINEnum,
            AllergenType.Molluscs => Allergen.MOLLUSCSEnum,
            _ => throw new ArgumentOutOfRangeException(nameof(allergen), $"Unexpected allergen value: {allergen}")
        };
    }


}
