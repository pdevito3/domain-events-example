namespace RecipeManagement.FunctionalTests.FunctionalTests.Authors;

using RecipeManagement.SharedTestHelpers.Fakes.Author;
using RecipeManagement.FunctionalTests.TestUtilities;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class GetAuthorListTests : TestBase
{
    [Test]
    public async Task get_author_list_returns_success()
    {
        // Arrange
        

        // Act
        var result = await _client.GetRequestAsync(ApiRoutes.Authors.GetList);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}