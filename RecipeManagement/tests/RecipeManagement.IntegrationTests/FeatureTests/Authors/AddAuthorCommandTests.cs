namespace RecipeManagement.IntegrationTests.FeatureTests.Authors;

using RecipeManagement.SharedTestHelpers.Fakes.Author;
using RecipeManagement.IntegrationTests.TestUtilities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using RecipeManagement.Domain.Authors.Features;
using static TestFixture;
using SharedKernel.Exceptions;
using RecipeManagement.SharedTestHelpers.Fakes.Recipe;

public class AddAuthorCommandTests : TestBase
{
    [Test]
    public async Task can_add_new_author_to_db()
    {
        // Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
        await InsertAsync(fakeRecipeOne);

        var fakeAuthorOne = new FakeAuthorForCreationDto()
            .RuleFor(a => a.RecipeId, _ => fakeRecipeOne.Id)
            .Generate();

        // Act
        var command = new AddAuthor.AddAuthorCommand(fakeAuthorOne);
        var authorReturned = await SendAsync(command);
        var authorCreated = await ExecuteDbContextAsync(db => db.Authors.SingleOrDefaultAsync());

        // Assert
        authorReturned.Should().BeEquivalentTo(fakeAuthorOne, options =>
            options.ExcludingMissingMembers());
        authorCreated.Should().BeEquivalentTo(fakeAuthorOne, options =>
            options.ExcludingMissingMembers());
    }
}