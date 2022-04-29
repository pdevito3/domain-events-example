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

public class AddRecipeProducerTests : TestBase
{
    [Test]
    public async Task can_produce_IRecipeAdded_message()
    {
        // Arrange
        var command = new AddRecipeProducer.AddRecipeProducerCommand();

        // Act
        await SendAsync(command);

        // Assert
        (await IsFaultyPublished<IRecipeAdded>()).Should().BeFalse();
        (await IsPublished<IRecipeAdded>()).Should().BeTrue();
    }
}