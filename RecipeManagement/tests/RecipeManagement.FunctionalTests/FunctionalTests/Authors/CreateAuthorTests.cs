namespace RecipeManagement.FunctionalTests.FunctionalTests.Authors;

using RecipeManagement.SharedTestHelpers.Fakes.Author;
using RecipeManagement.FunctionalTests.TestUtilities;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class CreateAuthorTests : TestBase
{
    [Test]
    public async Task create_author_returns_created_using_valid_dto()
    {
        // Arrange
        var fakeAuthor = new FakeAuthorForCreationDto { }.Generate();

        // Act
        var route = ApiRoutes.Authors.Create;
        var result = await _client.PostJsonRequestAsync(route, fakeAuthor);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}