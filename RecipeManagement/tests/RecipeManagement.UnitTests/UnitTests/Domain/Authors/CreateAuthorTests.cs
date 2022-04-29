namespace RecipeManagement.UnitTests.UnitTests.Domain.Authors;

using RecipeManagement.SharedTestHelpers.Fakes.Author;
using RecipeManagement.Domain.Authors;
using Bogus;
using FluentAssertions;
using NUnit.Framework;

public class CreateAuthorTests
{
    private readonly Faker _faker;

    public CreateAuthorTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_create_valid_author()
    {
        // Arrange + Act
        var fakeAuthor = Author.Create(new FakeAuthorForCreationDto().Generate());

        // Assert
        fakeAuthor.Should().NotBeNull();
    }
}