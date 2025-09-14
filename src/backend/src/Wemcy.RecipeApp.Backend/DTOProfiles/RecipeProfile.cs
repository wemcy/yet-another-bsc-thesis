using AutoMapper;
using Wemcy.RecipeApp.Backend.Api.Models;
using Wemcy.RecipeApp.Backend.Model;

namespace Wemcy.RecipeApp.Backend.DTOProfiles
{
    public class RecipeProfile : Profile
    {
        public RecipeProfile()
        {
            CreateMap<CreateRecipeDTO, Recipe>();
            CreateMap<Recipe, ReadRecipeDTO>();
            CreateMap<Api.Models.Allergen, AllergenType>().ReverseMap();
            CreateMap<Api.Models.Allergen, Model.Allergen>().ConvertUsing((e, c, ctx) =>
            {
                var m = ctx.Mapper.Map<AllergenType>(c);
                return new Model.Allergen() { Type = m };
            });

            CreateMap<Model.Allergen, Api.Models.Allergen>().ConvertUsing((e, c, ctx) =>
            {
                return ctx.Mapper.Map<Api.Models.Allergen>(e.Type);
 
            });


        }
    }
}
