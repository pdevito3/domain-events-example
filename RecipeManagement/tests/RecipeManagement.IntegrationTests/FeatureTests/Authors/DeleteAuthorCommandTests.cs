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

public class DeleteAuthorCommandTests : TestBase
{
    [Test]
    public async Task can_delete_author_from_db()
    {
        // Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
        await InsertAsync(fakeRecipeOne);

        var fakeAuthorOne = FakeAuthor.Generate(new FakeAuthorForCreationDto()
            .RuleFor(a => a.RecipeId, _ => fakeRecipeOne.Id)
            .Generate());
        await InsertAsync(fakeAuthorOne);
        var author = await ExecuteDbContextAsync(db => db.Authors.SingleOrDefaultAsync());
        var id = author.Id;

        // Act
        var command = new DeleteAuthor.DeleteAuthorCommand(id);
        await SendAsync(command);
        var authorResponse = await ExecuteDbContextAsync(db => db.Authors.ToListAsync());

        // Assert
        authorResponse.Count.Should().Be(0);
    }

    [Test]
    public async Task delete_author_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var command = new DeleteAuthor.DeleteAuthorCommand(badId);
        Func<Task> act = () => SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task can_softdelete_author_from_db()
    {
        // Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
        await InsertAsync(fakeRecipeOne);

        var fakeAuthorOne = FakeAuthor.Generate(new FakeAuthorForCreationDto()
            .RuleFor(a => a.RecipeId, _ => fakeRecipeOne.Id)
            .Generate());
        await InsertAsync(fakeAuthorOne);
        var author = await ExecuteDbContextAsync(db => db.Authors.SingleOrDefaultAsync());
        var id = author.Id;

        // Act
        var command = new DeleteAuthor.DeleteAuthorCommand(id);
        await SendAsync(command);
        var deletedAuthor = (await ExecuteDbContextAsync(db => db.Authors
            .IgnoreQueryFilters()
            .ToListAsync())
        ).FirstOrDefault();

        // Assert
        deletedAuthor?.IsDeleted.Should().BeTrue();
    }
}