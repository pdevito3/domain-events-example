namespace RecipeManagement.FunctionalTests.FunctionalTests.Authors;

using RecipeManagement.SharedTestHelpers.Fakes.Author;
using RecipeManagement.FunctionalTests.TestUtilities;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class UpdateAuthorRecordTests : TestBase
{
    [Test]
    public async Task put_author_returns_nocontent_when_entity_exists()
    {
        // Arrange
        var fakeAuthor = FakeAuthor.Generate(new FakeAuthorForCreationDto().Generate());
        var updatedAuthorDto = new FakeAuthorForUpdateDto { }.Generate();
        await InsertAsync(fakeAuthor);

        // Act
        var route = ApiRoutes.Authors.Put.Replace(ApiRoutes.Authors.Id, fakeAuthor.Id.ToString());
        var result = await _client.PutJsonRequestAsync(route, updatedAuthorDto);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}