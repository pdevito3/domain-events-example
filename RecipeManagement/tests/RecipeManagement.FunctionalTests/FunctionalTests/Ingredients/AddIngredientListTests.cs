namespace RecipeManagement.FunctionalTests.FunctionalTests.Ingredients;

using SharedKernel.Dtos.RecipeManagement.Ingredient;
using RecipeManagement.SharedTestHelpers.Fakes.Ingredient;
using RecipeManagement.SharedTestHelpers.Fakes.Recipe;
using RecipeManagement.FunctionalTests.TestUtilities;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class AddIngredientListTests : TestBase
{
    [Test]
    public async Task create_ingredient_list_returns_created_using_valid_dto()
    {
        // Arrange
        var fakeRecipe = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
        await InsertAsync(fakeRecipe);
        var fakeIngredientList = new List<IngredientForCreationDto> {new FakeIngredientForCreationDto { }.Generate()};

        // Act
        var route = ApiRoutes.Ingredients.CreateBatch;
        var result = await _client.PostJsonRequestAsync($"{route}?recipeid={fakeRecipe.Id}", fakeIngredientList);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }
    [Test]
    public async Task create_ingredient_list_returns_notfound_when_fk_doesnt_exist()
    {
        // Arrange
        var fakeIngredientList = new List<IngredientForCreationDto> {new FakeIngredientForCreationDto { }.Generate()};

        // Act
        var route = ApiRoutes.Ingredients.CreateBatch;
        var result = await _client.PostJsonRequestAsync($"{route}?recipeid={Guid.NewGuid()}", fakeIngredientList);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    [Test]
    public async Task create_ingredient_list_returns_badrequest_when_no_fk_param()
    {
        // Arrange
        var fakeIngredientList = new List<IngredientForCreationDto> {new FakeIngredientForCreationDto { }.Generate()};

        // Act
        var result = await _client.PostJsonRequestAsync(ApiRoutes.Ingredients.CreateBatch, fakeIngredientList);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}