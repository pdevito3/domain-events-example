namespace RecipeManagement.FunctionalTests.FunctionalTests.Recipes;

using RecipeManagement.SharedTestHelpers.Fakes.Recipe;
using RecipeManagement.FunctionalTests.TestUtilities;
using RecipeManagement.Domain;
using SharedKernel.Domain;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class GetRecipeTests : TestBase
{
    [Test]
    public async Task get_recipe_returns_success_when_entity_exists_using_valid_auth_credentials()
    {
        // Arrange
        var fakeRecipe = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());

        _client.AddAuth(new[] {Roles.SuperAdmin});
        await InsertAsync(fakeRecipe);

        // Act
        var route = ApiRoutes.Recipes.GetRecord.Replace(ApiRoutes.Recipes.Id, fakeRecipe.Id.ToString());
        var result = await _client.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
            
    [Test]
    public async Task get_recipe_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeRecipe = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());

        await InsertAsync(fakeRecipe);

        // Act
        var route = ApiRoutes.Recipes.GetRecord.Replace(ApiRoutes.Recipes.Id, fakeRecipe.Id.ToString());
        var result = await _client.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task get_recipe_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeRecipe = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
        _client.AddAuth();

        await InsertAsync(fakeRecipe);

        // Act
        var route = ApiRoutes.Recipes.GetRecord.Replace(ApiRoutes.Recipes.Id, fakeRecipe.Id.ToString());
        var result = await _client.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}