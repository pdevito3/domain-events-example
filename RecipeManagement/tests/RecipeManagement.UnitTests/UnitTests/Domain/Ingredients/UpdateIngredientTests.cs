namespace RecipeManagement.UnitTests.UnitTests.Domain.Ingredients;

using RecipeManagement.SharedTestHelpers.Fakes.Ingredient;
using RecipeManagement.Domain.Ingredients;
using Bogus;
using FluentAssertions;
using NUnit.Framework;

public class UpdateIngredientTests
{
    private readonly Faker _faker;

    public UpdateIngredientTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_update_ingredient()
    {
        // Arrange
        var fakeIngredient = FakeIngredient.Generate();
        var updatedIngredient = new FakeIngredientForUpdateDto().Generate();
        
        // Act
        fakeIngredient.Update(updatedIngredient);

        // Assert
        fakeIngredient.Should().BeEquivalentTo(updatedIngredient, options =>
            options.ExcludingMissingMembers());
    }
}