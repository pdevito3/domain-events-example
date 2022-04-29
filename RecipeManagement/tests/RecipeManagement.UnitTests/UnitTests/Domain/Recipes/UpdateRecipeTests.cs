namespace RecipeManagement.UnitTests.UnitTests.Domain.Recipes;

using RecipeManagement.SharedTestHelpers.Fakes.Recipe;
using RecipeManagement.Domain.Recipes;
using Bogus;
using FluentAssertions;
using NUnit.Framework;

public class UpdateRecipeTests
{
    private readonly Faker _faker;

    public UpdateRecipeTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_update_recipe()
    {
        // Arrange
        var fakeRecipe = FakeRecipe.Generate();
        var updatedRecipe = new FakeRecipeForUpdateDto().Generate();
        
        // Act
        fakeRecipe.Update(updatedRecipe);

        // Assert
        fakeRecipe.Should().BeEquivalentTo(updatedRecipe, options =>
            options.ExcludingMissingMembers());
    }
}