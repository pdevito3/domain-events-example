namespace RecipeManagement.IntegrationTests.FeatureTests.Ingredients;

using RecipeManagement.SharedTestHelpers.Fakes.Ingredient;
using RecipeManagement.IntegrationTests.TestUtilities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using RecipeManagement.Domain.Ingredients.Features;
using static TestFixture;
using SharedKernel.Exceptions;
using RecipeManagement.SharedTestHelpers.Fakes.Recipe;

public class AddIngredientCommandTests : TestBase
{
    [Test]
    public async Task can_add_new_ingredient_to_db()
    {
        // Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
        await InsertAsync(fakeRecipeOne);

        var fakeIngredientOne = new FakeIngredientForCreationDto()
            .RuleFor(i => i.RecipeId, _ => fakeRecipeOne.Id)
            .Generate();

        // Act
        var command = new AddIngredient.AddIngredientCommand(fakeIngredientOne);
        var ingredientReturned = await SendAsync(command);
        var ingredientCreated = await ExecuteDbContextAsync(db => db.Ingredients.SingleOrDefaultAsync());

        // Assert
        ingredientReturned.Should().BeEquivalentTo(fakeIngredientOne, options =>
            options.ExcludingMissingMembers());
        ingredientCreated.Should().BeEquivalentTo(fakeIngredientOne, options =>
            options.ExcludingMissingMembers());
    }
}