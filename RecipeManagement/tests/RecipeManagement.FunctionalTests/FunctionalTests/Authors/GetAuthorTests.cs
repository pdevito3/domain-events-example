namespace RecipeManagement.FunctionalTests.FunctionalTests.Authors;

using RecipeManagement.SharedTestHelpers.Fakes.Author;
using RecipeManagement.FunctionalTests.TestUtilities;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class GetAuthorTests : TestBase
{
    [Test]
    public async Task get_author_returns_success_when_entity_exists()
    {
        // Arrange
        var fakeAuthor = FakeAuthor.Generate(new FakeAuthorForCreationDto().Generate());
        await InsertAsync(fakeAuthor);

        // Act
        var route = ApiRoutes.Authors.GetRecord.Replace(ApiRoutes.Authors.Id, fakeAuthor.Id.ToString());
        var result = await _client.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}