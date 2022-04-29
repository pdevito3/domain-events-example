namespace RecipeManagement.IntegrationTests.FeatureTests.Recipes;

using RecipeManagement.SharedTestHelpers.Fakes.Recipe;
using RecipeManagement.IntegrationTests.TestUtilities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using RecipeManagement.Domain.Recipes.Features;
using static TestFixture;
using SharedKernel.Exceptions;
using RecipeManagement.SharedTestHelpers.Fakes.Author;

public class AddRecipeCommandTests : TestBase
{
    [Test]
    public async Task can_add_new_recipe_to_db()
    {
        // Arrange
        var fakeRecipeOne = new FakeRecipeForCreationDto().Generate();

        // Act
        var command = new AddRecipe.AddRecipeCommand(fakeRecipeOne);
        var recipeReturned = await SendAsync(command);
        var recipeCreated = await ExecuteDbContextAsync(db => db.Recipes.SingleOrDefaultAsync());

        // Assert
        recipeReturned.Should().BeEquivalentTo(fakeRecipeOne, options =>
            options.ExcludingMissingMembers());
        recipeCreated.Should().BeEquivalentTo(fakeRecipeOne, options =>
            options.ExcludingMissingMembers());
    }
}