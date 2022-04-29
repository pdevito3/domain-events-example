namespace RecipeManagement.IntegrationTests.FeatureTests.Recipes;

using SharedKernel.Dtos.RecipeManagement.Recipe;
using RecipeManagement.SharedTestHelpers.Fakes.Recipe;
using SharedKernel.Exceptions;
using RecipeManagement.Domain.Recipes.Features;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;
using RecipeManagement.SharedTestHelpers.Fakes.Author;

public class RecipeListQueryTests : TestBase
{
    
    [Test]
    public async Task can_get_recipe_list()
    {
        // Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
        var fakeRecipeTwo = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
        var queryParameters = new RecipeParametersDto();

        await InsertAsync(fakeRecipeOne, fakeRecipeTwo);

        // Act
        var query = new GetRecipeList.RecipeListQuery(queryParameters);
        var recipes = await SendAsync(query);

        // Assert
        recipes.Should().HaveCount(2);
    }
    
    [Test]
    public async Task can_get_recipe_list_with_expected_page_size_and_number()
    {
        //Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
        var fakeRecipeTwo = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
        var fakeRecipeThree = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
        var queryParameters = new RecipeParametersDto() { PageSize = 1, PageNumber = 2 };

        await InsertAsync(fakeRecipeOne, fakeRecipeTwo, fakeRecipeThree);

        //Act
        var query = new GetRecipeList.RecipeListQuery(queryParameters);
        var recipes = await SendAsync(query);

        // Assert
        recipes.Should().HaveCount(1);
    }
    
    [Test]
    public async Task can_get_sorted_list_of_recipe_by_Title_in_asc_order()
    {
        //Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto()
            .RuleFor(r => r.Title, _ => "bravo")
            .Generate());
        var fakeRecipeTwo = FakeRecipe.Generate(new FakeRecipeForCreationDto()
            .RuleFor(r => r.Title, _ => "alpha")
            .Generate());
        var queryParameters = new RecipeParametersDto() { SortOrder = "Title" };

        await InsertAsync(fakeRecipeOne, fakeRecipeTwo);

        //Act
        var query = new GetRecipeList.RecipeListQuery(queryParameters);
        var recipes = await SendAsync(query);

        // Assert
        recipes
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeRecipeTwo, options =>
                options.ExcludingMissingMembers());
        recipes
            .Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeRecipeOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_recipe_by_Title_in_desc_order()
    {
        //Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto()
            .RuleFor(r => r.Title, _ => "alpha")
            .Generate());
        var fakeRecipeTwo = FakeRecipe.Generate(new FakeRecipeForCreationDto()
            .RuleFor(r => r.Title, _ => "bravo")
            .Generate());
        var queryParameters = new RecipeParametersDto() { SortOrder = "-Title" };

        await InsertAsync(fakeRecipeOne, fakeRecipeTwo);

        //Act
        var query = new GetRecipeList.RecipeListQuery(queryParameters);
        var recipes = await SendAsync(query);

        // Assert
        recipes
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeRecipeTwo, options =>
                options.ExcludingMissingMembers());
        recipes
            .Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeRecipeOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_recipe_by_Directions_in_asc_order()
    {
        //Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto()
            .RuleFor(r => r.Directions, _ => "bravo")
            .Generate());
        var fakeRecipeTwo = FakeRecipe.Generate(new FakeRecipeForCreationDto()
            .RuleFor(r => r.Directions, _ => "alpha")
            .Generate());
        var queryParameters = new RecipeParametersDto() { SortOrder = "Directions" };

        await InsertAsync(fakeRecipeOne, fakeRecipeTwo);

        //Act
        var query = new GetRecipeList.RecipeListQuery(queryParameters);
        var recipes = await SendAsync(query);

        // Assert
        recipes
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeRecipeTwo, options =>
                options.ExcludingMissingMembers());
        recipes
            .Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeRecipeOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_recipe_by_Directions_in_desc_order()
    {
        //Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto()
            .RuleFor(r => r.Directions, _ => "alpha")
            .Generate());
        var fakeRecipeTwo = FakeRecipe.Generate(new FakeRecipeForCreationDto()
            .RuleFor(r => r.Directions, _ => "bravo")
            .Generate());
        var queryParameters = new RecipeParametersDto() { SortOrder = "-Directions" };

        await InsertAsync(fakeRecipeOne, fakeRecipeTwo);

        //Act
        var query = new GetRecipeList.RecipeListQuery(queryParameters);
        var recipes = await SendAsync(query);

        // Assert
        recipes
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeRecipeTwo, options =>
                options.ExcludingMissingMembers());
        recipes
            .Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeRecipeOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_recipe_by_Rating_in_asc_order()
    {
        //Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto()
            .RuleFor(r => r.Rating, _ => 2)
            .Generate());
        var fakeRecipeTwo = FakeRecipe.Generate(new FakeRecipeForCreationDto()
            .RuleFor(r => r.Rating, _ => 1)
            .Generate());
        var queryParameters = new RecipeParametersDto() { SortOrder = "Rating" };

        await InsertAsync(fakeRecipeOne, fakeRecipeTwo);

        //Act
        var query = new GetRecipeList.RecipeListQuery(queryParameters);
        var recipes = await SendAsync(query);

        // Assert
        recipes
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeRecipeTwo, options =>
                options.ExcludingMissingMembers());
        recipes
            .Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeRecipeOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_recipe_by_Rating_in_desc_order()
    {
        //Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto()
            .RuleFor(r => r.Rating, _ => 1)
            .Generate());
        var fakeRecipeTwo = FakeRecipe.Generate(new FakeRecipeForCreationDto()
            .RuleFor(r => r.Rating, _ => 2)
            .Generate());
        var queryParameters = new RecipeParametersDto() { SortOrder = "-Rating" };

        await InsertAsync(fakeRecipeOne, fakeRecipeTwo);

        //Act
        var query = new GetRecipeList.RecipeListQuery(queryParameters);
        var recipes = await SendAsync(query);

        // Assert
        recipes
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeRecipeTwo, options =>
                options.ExcludingMissingMembers());
        recipes
            .Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeRecipeOne, options =>
                options.ExcludingMissingMembers());
    }

    
    [Test]
    public async Task can_filter_recipe_list_using_Title()
    {
        //Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto()
            .RuleFor(r => r.Title, _ => "alpha")
            .Generate());
        var fakeRecipeTwo = FakeRecipe.Generate(new FakeRecipeForCreationDto()
            .RuleFor(r => r.Title, _ => "bravo")
            .Generate());
        var queryParameters = new RecipeParametersDto() { Filters = $"Title == {fakeRecipeTwo.Title}" };

        await InsertAsync(fakeRecipeOne, fakeRecipeTwo);

        //Act
        var query = new GetRecipeList.RecipeListQuery(queryParameters);
        var recipes = await SendAsync(query);

        // Assert
        recipes.Should().HaveCount(1);
        recipes
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeRecipeTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_recipe_list_using_Directions()
    {
        //Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto()
            .RuleFor(r => r.Directions, _ => "alpha")
            .Generate());
        var fakeRecipeTwo = FakeRecipe.Generate(new FakeRecipeForCreationDto()
            .RuleFor(r => r.Directions, _ => "bravo")
            .Generate());
        var queryParameters = new RecipeParametersDto() { Filters = $"Directions == {fakeRecipeTwo.Directions}" };

        await InsertAsync(fakeRecipeOne, fakeRecipeTwo);

        //Act
        var query = new GetRecipeList.RecipeListQuery(queryParameters);
        var recipes = await SendAsync(query);

        // Assert
        recipes.Should().HaveCount(1);
        recipes
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeRecipeTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_recipe_list_using_Rating()
    {
        //Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto()
            .RuleFor(r => r.Rating, _ => 1)
            .Generate());
        var fakeRecipeTwo = FakeRecipe.Generate(new FakeRecipeForCreationDto()
            .RuleFor(r => r.Rating, _ => 2)
            .Generate());
        var queryParameters = new RecipeParametersDto() { Filters = $"Rating == {fakeRecipeTwo.Rating}" };

        await InsertAsync(fakeRecipeOne, fakeRecipeTwo);

        //Act
        var query = new GetRecipeList.RecipeListQuery(queryParameters);
        var recipes = await SendAsync(query);

        // Assert
        recipes.Should().HaveCount(1);
        recipes
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeRecipeTwo, options =>
                options.ExcludingMissingMembers());
    }

}