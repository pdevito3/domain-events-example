namespace RecipeManagement.IntegrationTests.FeatureTests.Ingredients;

using SharedKernel.Dtos.RecipeManagement.Ingredient;
using RecipeManagement.SharedTestHelpers.Fakes.Ingredient;
using RecipeManagement.SharedTestHelpers.Fakes.Recipe;
using RecipeManagement.IntegrationTests.TestUtilities;
using RecipeManagement.Domain.Ingredients.Features;
using SharedKernel.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;
using RecipeManagement.SharedTestHelpers.Fakes.Recipe;

public class AddIngredientListCommandTests : TestBase
{
    [Test]
    public async Task can_add_new_ingredient_list_to_db()
    {
        // Arrange
        var fakeRecipe = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
        await InsertAsync(fakeRecipe);
        var fakeIngredientOne = new FakeIngredientForCreationDto().Generate();

        // Act
        var command = new AddIngredientList.AddIngredientListCommand(new List<IngredientForCreationDto>() {fakeIngredientOne}, fakeRecipe.Id);
        var ingredientReturned = await SendAsync(command);
        var ingredientCreated = await ExecuteDbContextAsync(db => db.Ingredients.SingleOrDefaultAsync());

        // Assert
        ingredientReturned.FirstOrDefault().Should().BeEquivalentTo(fakeIngredientOne, options =>
            options.ExcludingMissingMembers());
        ingredientCreated.Should().BeEquivalentTo(fakeIngredientOne, options =>
            options.ExcludingMissingMembers());
    }
}