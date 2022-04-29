namespace RecipeManagement.SharedTestHelpers.Fakes.Ingredient;

using AutoBogus;
using RecipeManagement.Domain.Ingredients;
using SharedKernel.Dtos.RecipeManagement.Ingredient;

public class FakeIngredient
{
    public static Ingredient Generate(IngredientForCreationDto ingredientForCreationDto)
    {
        return Ingredient.Create(ingredientForCreationDto);
    }

    public static Ingredient Generate()
    {
        return Ingredient.Create(new FakeIngredientForCreationDto().Generate());
    }
}