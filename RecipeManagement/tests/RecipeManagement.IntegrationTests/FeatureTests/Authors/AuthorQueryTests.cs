namespace RecipeManagement.IntegrationTests.FeatureTests.Authors;

using RecipeManagement.SharedTestHelpers.Fakes.Author;
using RecipeManagement.IntegrationTests.TestUtilities;
using RecipeManagement.Domain.Authors.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using static TestFixture;
using RecipeManagement.SharedTestHelpers.Fakes.Recipe;

public class AuthorQueryTests : TestBase
{
    [Test]
    public async Task can_get_existing_author_with_accurate_props()
    {
        // Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
        await InsertAsync(fakeRecipeOne);

        var fakeAuthorOne = FakeAuthor.Generate(new FakeAuthorForCreationDto()
            .RuleFor(a => a.RecipeId, _ => fakeRecipeOne.Id)
            .Generate());
        await InsertAsync(fakeAuthorOne);

        // Act
        var query = new GetAuthor.AuthorQuery(fakeAuthorOne.Id);
        var authors = await SendAsync(query);

        // Assert
        authors.Should().BeEquivalentTo(fakeAuthorOne, options =>
            options.ExcludingMissingMembers());
    }

    [Test]
    public async Task get_author_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var query = new GetAuthor.AuthorQuery(badId);
        Func<Task> act = () => SendAsync(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}