namespace RecipeManagement.SharedTestHelpers.Fakes.Recipe;

using AutoBogus;
using SharedKernel.Dtos.RecipeManagement.Recipe;

// or replace 'AutoFaker' with 'Faker' along with your own rules if you don't want all fields to be auto faked
public class FakeRecipeForUpdateDto : AutoFaker<RecipeForUpdateDto>
{
    public FakeRecipeForUpdateDto()
    {
        // if you want default values on any of your properties (e.g. an int between a certain range or a date always in the past), you can add `RuleFor` lines describing those defaults
        //RuleFor(r => r.ExampleIntProperty, r => r.Random.Number(50, 100000));
        //RuleFor(r => r.ExampleDateProperty, r => r.Date.Past());
    }
}