namespace RecipeManagement.IntegrationTests.FeatureTests.Recipes;

using RecipeManagement.SharedTestHelpers.Fakes.Recipe;
using RecipeManagement.IntegrationTests.TestUtilities;
using RecipeManagement.Domain.Recipes.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using static TestFixture;
using RecipeManagement.SharedTestHelpers.Fakes.Author;

public class RecipeQueryTests : TestBase
{
    [Test]
    public async Task can_get_existing_recipe_with_accurate_props()
    {
        // Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
        await InsertAsync(fakeRecipeOne);

        // Act
        var query = new GetRecipe.RecipeQuery(fakeRecipeOne.Id);
        var recipes = await SendAsync(query);

        // Assert
        recipes.Should().BeEquivalentTo(fakeRecipeOne, options =>
            options.ExcludingMissingMembers());
    }

    [Test]
    public async Task get_recipe_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var query = new GetRecipe.RecipeQuery(badId);
        Func<Task> act = () => SendAsync(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}