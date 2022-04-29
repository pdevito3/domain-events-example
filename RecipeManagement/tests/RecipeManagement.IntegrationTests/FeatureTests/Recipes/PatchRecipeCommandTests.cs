namespace RecipeManagement.IntegrationTests.FeatureTests.Recipes;

using RecipeManagement.SharedTestHelpers.Fakes.Recipe;
using RecipeManagement.IntegrationTests.TestUtilities;
using SharedKernel.Dtos.RecipeManagement.Recipe;
using SharedKernel.Exceptions;
using RecipeManagement.Domain.Recipes.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using static TestFixture;
using RecipeManagement.SharedTestHelpers.Fakes.Author;

public class PatchRecipeCommandTests : TestBase
{
    [Test]
    public async Task can_patch_existing_recipe_in_db()
    {
        // Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
        await InsertAsync(fakeRecipeOne);
        var recipe = await ExecuteDbContextAsync(db => db.Recipes.SingleOrDefaultAsync());
        var id = recipe.Id;

        var patchDoc = new JsonPatchDocument<RecipeForUpdateDto>();
        var newValue = "Easily Identified Value For Test";
        patchDoc.Replace(r => r.Title, newValue);

        // Act
        var command = new PatchRecipe.PatchRecipeCommand(id, patchDoc);
        await SendAsync(command);
        var updatedRecipe = await ExecuteDbContextAsync(db => db.Recipes.Where(r => r.Id == id).SingleOrDefaultAsync());

        // Assert
        updatedRecipe.Title.Should().Be(newValue);
    }
    
    [Test]
    public async Task patch_recipe_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();
        var patchDoc = new JsonPatchDocument<RecipeForUpdateDto>();

        // Act
        var command = new PatchRecipe.PatchRecipeCommand(badId, patchDoc);
        Func<Task> act = () => SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task passing_null_patchdoc_throws_validationexception()
    {
        // Arrange
        var randomId = Guid.NewGuid();

        // Act
        var command = new PatchRecipe.PatchRecipeCommand(randomId, null);
        Func<Task> act = () => SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }
}