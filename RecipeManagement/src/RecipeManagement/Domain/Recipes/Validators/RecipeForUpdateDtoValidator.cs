namespace RecipeManagement.Domain.Recipes.Validators;

using SharedKernel.Dtos.RecipeManagement.Recipe;
using FluentValidation;

public class RecipeForUpdateDtoValidator: RecipeForManipulationDtoValidator<RecipeForUpdateDto>
{
    public RecipeForUpdateDtoValidator()
    {
        // add fluent validation rules that should only be run on update operations here
        //https://fluentvalidation.net/
    }
}