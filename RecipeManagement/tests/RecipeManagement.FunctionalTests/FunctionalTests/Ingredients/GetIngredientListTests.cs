namespace RecipeManagement.FunctionalTests.FunctionalTests.Ingredients;

using RecipeManagement.SharedTestHelpers.Fakes.Ingredient;
using RecipeManagement.FunctionalTests.TestUtilities;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class GetIngredientListTests : TestBase
{
    [Test]
    public async Task get_ingredient_list_returns_success()
    {
        // Arrange
        

        // Act
        var result = await _client.GetRequestAsync(ApiRoutes.Ingredients.GetList);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}