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

public class DeleteRecipeCommandTests : TestBase
{
    [Test]
    public async Task can_delete_recipe_from_db()
    {
        // Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
        await InsertAsync(fakeRecipeOne);
        var recipe = await ExecuteDbContextAsync(db => db.Recipes.SingleOrDefaultAsync());
        var id = recipe.Id;

        // Act
        var command = new DeleteRecipe.DeleteRecipeCommand(id);
        await SendAsync(command);
        var recipeResponse = await ExecuteDbContextAsync(db => db.Recipes.ToListAsync());

        // Assert
        recipeResponse.Count.Should().Be(0);
    }

    [Test]
    public async Task delete_recipe_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var command = new DeleteRecipe.DeleteRecipeCommand(badId);
        Func<Task> act = () => SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task can_softdelete_recipe_from_db()
    {
        // Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
        await InsertAsync(fakeRecipeOne);
        var recipe = await ExecuteDbContextAsync(db => db.Recipes.SingleOrDefaultAsync());
        var id = recipe.Id;

        // Act
        var command = new DeleteRecipe.DeleteRecipeCommand(id);
        await SendAsync(command);
        var deletedRecipe = (await ExecuteDbContextAsync(db => db.Recipes
            .IgnoreQueryFilters()
            .ToListAsync())
        ).FirstOrDefault();

        // Assert
        deletedRecipe?.IsDeleted.Should().BeTrue();
    }
}