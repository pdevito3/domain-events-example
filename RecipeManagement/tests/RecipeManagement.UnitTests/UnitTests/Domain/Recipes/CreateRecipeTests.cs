namespace RecipeManagement.UnitTests.UnitTests.Domain.Recipes;

using RecipeManagement.SharedTestHelpers.Fakes.Recipe;
using RecipeManagement.Domain.Recipes;
using Bogus;
using FluentAssertions;
using NUnit.Framework;

public class CreateRecipeTests
{
    private readonly Faker _faker;

    public CreateRecipeTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_create_valid_recipe()
    {
        // Arrange + Act
        var fakeRecipe = Recipe.Create(new FakeRecipeForCreationDto().Generate());

        // Assert
        fakeRecipe.Should().NotBeNull();
    }
}