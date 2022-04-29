namespace RecipeManagement.UnitTests.UnitTests.Domain.Ingredients;

using RecipeManagement.SharedTestHelpers.Fakes.Ingredient;
using RecipeManagement.Domain.Ingredients;
using Bogus;
using FluentAssertions;
using NUnit.Framework;

public class CreateIngredientTests
{
    private readonly Faker _faker;

    public CreateIngredientTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_create_valid_ingredient()
    {
        // Arrange + Act
        var fakeIngredient = Ingredient.Create(new FakeIngredientForCreationDto().Generate());

        // Assert
        fakeIngredient.Should().NotBeNull();
    }
}