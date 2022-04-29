namespace RecipeManagement.Domain.Recipes.Mappings;

using SharedKernel.Dtos.RecipeManagement.Recipe;
using AutoMapper;
using RecipeManagement.Domain.Recipes;

public class RecipeProfile : Profile
{
    public RecipeProfile()
    {
        //createmap<to this, from this>
        CreateMap<Recipe, RecipeDto>()
            .ReverseMap();
        CreateMap<RecipeForCreationDto, Recipe>();
        CreateMap<RecipeForUpdateDto, Recipe>()
            .ReverseMap();
    }
}