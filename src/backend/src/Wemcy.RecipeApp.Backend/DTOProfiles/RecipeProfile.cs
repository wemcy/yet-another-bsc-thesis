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
            CreateMap<Api.Models.Allergen, Model.Allergen>().ConvertUsing((src, _, ctx) =>
            {
                var t = ctx.Mapper.Map<AllergenType>(src);
                return new Model.Allergen { Type = t };
            });
            CreateMap<Model.Allergen, Api.Models.Allergen>().ConvertUsing((src, _, ctx) =>
            {
                return ctx.Mapper.Map<Api.Models.Allergen>(src.Type);
            });
        }
    }
}
