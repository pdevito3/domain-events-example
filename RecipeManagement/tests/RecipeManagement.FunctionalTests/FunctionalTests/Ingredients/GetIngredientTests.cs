namespace RecipeManagement.FunctionalTests.FunctionalTests.Ingredients;

using RecipeManagement.SharedTestHelpers.Fakes.Ingredient;
using RecipeManagement.FunctionalTests.TestUtilities;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class GetIngredientTests : TestBase
{
    [Test]
    public async Task get_ingredient_returns_success_when_entity_exists()
    {
        // Arrange
        var fakeIngredient = FakeIngredient.Generate(new FakeIngredientForCreationDto().Generate());
        await InsertAsync(fakeIngredient);

        // Act
        var route = ApiRoutes.Ingredients.GetRecord.Replace(ApiRoutes.Ingredients.Id, fakeIngredient.Id.ToString());
        var result = await _client.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}