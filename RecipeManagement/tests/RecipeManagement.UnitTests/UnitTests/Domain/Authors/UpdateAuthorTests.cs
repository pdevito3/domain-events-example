namespace RecipeManagement.UnitTests.UnitTests.Domain.Authors;

using RecipeManagement.SharedTestHelpers.Fakes.Author;
using RecipeManagement.Domain.Authors;
using Bogus;
using FluentAssertions;
using NUnit.Framework;

public class UpdateAuthorTests
{
    private readonly Faker _faker;

    public UpdateAuthorTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_update_author()
    {
        // Arrange
        var fakeAuthor = FakeAuthor.Generate();
        var updatedAuthor = new FakeAuthorForUpdateDto().Generate();
        
        // Act
        fakeAuthor.Update(updatedAuthor);

        // Assert
        fakeAuthor.Should().BeEquivalentTo(updatedAuthor, options =>
            options.ExcludingMissingMembers());
    }
}