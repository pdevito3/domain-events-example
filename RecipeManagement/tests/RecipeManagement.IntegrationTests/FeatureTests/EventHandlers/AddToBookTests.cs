namespace RecipeManagement.IntegrationTests.FeatureTests.EventHandlers;

using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Testing;
using SharedKernel.Messages;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RecipeManagement.Domain.Recipes.Features;
using RecipeManagement.IntegrationTests.TestUtilities;
using static TestFixture;

public class AddToBookTests : TestBase
{
    [Test]
    public async Task can_consume_IRecipeAdded_message()
    {
        // Arrange
        var message = new Mock<IRecipeAdded>();

        // Act
        await PublishMessage<IRecipeAdded>(message);

        // Assert
        (await IsConsumed<IRecipeAdded>()).Should().Be(true);
        (await IsConsumed<IRecipeAdded, AddToBook>()).Should().Be(true);
    }
}