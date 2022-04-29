namespace RecipeManagement.FunctionalTests.FunctionalTests.Authors;

using RecipeManagement.SharedTestHelpers.Fakes.Author;
using RecipeManagement.FunctionalTests.TestUtilities;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class DeleteAuthorTests : TestBase
{
    [Test]
    public async Task delete_author_returns_nocontent_when_entity_exists()
    {
        // Arrange
        var fakeAuthor = FakeAuthor.Generate(new FakeAuthorForCreationDto().Generate());
        await InsertAsync(fakeAuthor);

        // Act
        var route = ApiRoutes.Authors.Delete.Replace(ApiRoutes.Authors.Id, fakeAuthor.Id.ToString());
        var result = await _client.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}