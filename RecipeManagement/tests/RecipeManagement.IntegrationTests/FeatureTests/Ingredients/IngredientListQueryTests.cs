namespace RecipeManagement.IntegrationTests.FeatureTests.Ingredients;

using SharedKernel.Dtos.RecipeManagement.Ingredient;
using RecipeManagement.SharedTestHelpers.Fakes.Ingredient;
using SharedKernel.Exceptions;
using RecipeManagement.Domain.Ingredients.Features;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;
using RecipeManagement.SharedTestHelpers.Fakes.Recipe;

public class IngredientListQueryTests : TestBase
{
    
    [Test]
    public async Task can_get_ingredient_list()
    {
        // Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    var fakeRecipeTwo = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    await InsertAsync(fakeRecipeOne, fakeRecipeTwo);

        var fakeIngredientOne = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.RecipeId, _ => fakeRecipeOne.Id)
            .Generate());
        var fakeIngredientTwo = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.RecipeId, _ => fakeRecipeTwo.Id)
            .Generate());
        var queryParameters = new IngredientParametersDto();

        await InsertAsync(fakeIngredientOne, fakeIngredientTwo);

        // Act
        var query = new GetIngredientList.IngredientListQuery(queryParameters);
        var ingredients = await SendAsync(query);

        // Assert
        ingredients.Should().HaveCount(2);
    }
    
    [Test]
    public async Task can_get_ingredient_list_with_expected_page_size_and_number()
    {
        //Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    var fakeRecipeTwo = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    var fakeRecipeThree = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    await InsertAsync(fakeRecipeOne, fakeRecipeTwo, fakeRecipeThree);

        var fakeIngredientOne = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.RecipeId, _ => fakeRecipeOne.Id)
            .Generate());
        var fakeIngredientTwo = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.RecipeId, _ => fakeRecipeTwo.Id)
            .Generate());
        var fakeIngredientThree = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.RecipeId, _ => fakeRecipeThree.Id)
            .Generate());
        var queryParameters = new IngredientParametersDto() { PageSize = 1, PageNumber = 2 };

        await InsertAsync(fakeIngredientOne, fakeIngredientTwo, fakeIngredientThree);

        //Act
        var query = new GetIngredientList.IngredientListQuery(queryParameters);
        var ingredients = await SendAsync(query);

        // Assert
        ingredients.Should().HaveCount(1);
    }
    
    [Test]
    public async Task can_get_sorted_list_of_ingredient_by_Name_in_asc_order()
    {
        //Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    var fakeRecipeTwo = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    await InsertAsync(fakeRecipeOne, fakeRecipeTwo);

        var fakeIngredientOne = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.Name, _ => "bravo")
            .RuleFor(i => i.RecipeId, _ => fakeRecipeOne.Id)            
            .Generate());
        var fakeIngredientTwo = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.Name, _ => "alpha")
            .RuleFor(i => i.RecipeId, _ => fakeRecipeTwo.Id)            
            .Generate());
        var queryParameters = new IngredientParametersDto() { SortOrder = "Name" };

        await InsertAsync(fakeIngredientOne, fakeIngredientTwo);

        //Act
        var query = new GetIngredientList.IngredientListQuery(queryParameters);
        var ingredients = await SendAsync(query);

        // Assert
        ingredients
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeIngredientTwo, options =>
                options.ExcludingMissingMembers());
        ingredients
            .Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeIngredientOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_ingredient_by_Name_in_desc_order()
    {
        //Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    var fakeRecipeTwo = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    await InsertAsync(fakeRecipeOne, fakeRecipeTwo);

        var fakeIngredientOne = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.Name, _ => "alpha")
            .RuleFor(i => i.RecipeId, _ => fakeRecipeOne.Id)            
            .Generate());
        var fakeIngredientTwo = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.Name, _ => "bravo")
            .RuleFor(i => i.RecipeId, _ => fakeRecipeTwo.Id)            
            .Generate());
        var queryParameters = new IngredientParametersDto() { SortOrder = "-Name" };

        await InsertAsync(fakeIngredientOne, fakeIngredientTwo);

        //Act
        var query = new GetIngredientList.IngredientListQuery(queryParameters);
        var ingredients = await SendAsync(query);

        // Assert
        ingredients
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeIngredientTwo, options =>
                options.ExcludingMissingMembers());
        ingredients
            .Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeIngredientOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_ingredient_by_Quantity_in_asc_order()
    {
        //Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    var fakeRecipeTwo = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    await InsertAsync(fakeRecipeOne, fakeRecipeTwo);

        var fakeIngredientOne = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.Quantity, _ => "bravo")
            .RuleFor(i => i.RecipeId, _ => fakeRecipeOne.Id)            
            .Generate());
        var fakeIngredientTwo = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.Quantity, _ => "alpha")
            .RuleFor(i => i.RecipeId, _ => fakeRecipeTwo.Id)            
            .Generate());
        var queryParameters = new IngredientParametersDto() { SortOrder = "Quantity" };

        await InsertAsync(fakeIngredientOne, fakeIngredientTwo);

        //Act
        var query = new GetIngredientList.IngredientListQuery(queryParameters);
        var ingredients = await SendAsync(query);

        // Assert
        ingredients
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeIngredientTwo, options =>
                options.ExcludingMissingMembers());
        ingredients
            .Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeIngredientOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_ingredient_by_Quantity_in_desc_order()
    {
        //Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    var fakeRecipeTwo = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    await InsertAsync(fakeRecipeOne, fakeRecipeTwo);

        var fakeIngredientOne = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.Quantity, _ => "alpha")
            .RuleFor(i => i.RecipeId, _ => fakeRecipeOne.Id)            
            .Generate());
        var fakeIngredientTwo = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.Quantity, _ => "bravo")
            .RuleFor(i => i.RecipeId, _ => fakeRecipeTwo.Id)            
            .Generate());
        var queryParameters = new IngredientParametersDto() { SortOrder = "-Quantity" };

        await InsertAsync(fakeIngredientOne, fakeIngredientTwo);

        //Act
        var query = new GetIngredientList.IngredientListQuery(queryParameters);
        var ingredients = await SendAsync(query);

        // Assert
        ingredients
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeIngredientTwo, options =>
                options.ExcludingMissingMembers());
        ingredients
            .Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeIngredientOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_ingredient_by_Measure_in_asc_order()
    {
        //Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    var fakeRecipeTwo = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    await InsertAsync(fakeRecipeOne, fakeRecipeTwo);

        var fakeIngredientOne = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.Measure, _ => "bravo")
            .RuleFor(i => i.RecipeId, _ => fakeRecipeOne.Id)            
            .Generate());
        var fakeIngredientTwo = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.Measure, _ => "alpha")
            .RuleFor(i => i.RecipeId, _ => fakeRecipeTwo.Id)            
            .Generate());
        var queryParameters = new IngredientParametersDto() { SortOrder = "Measure" };

        await InsertAsync(fakeIngredientOne, fakeIngredientTwo);

        //Act
        var query = new GetIngredientList.IngredientListQuery(queryParameters);
        var ingredients = await SendAsync(query);

        // Assert
        ingredients
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeIngredientTwo, options =>
                options.ExcludingMissingMembers());
        ingredients
            .Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeIngredientOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_ingredient_by_Measure_in_desc_order()
    {
        //Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    var fakeRecipeTwo = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    await InsertAsync(fakeRecipeOne, fakeRecipeTwo);

        var fakeIngredientOne = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.Measure, _ => "alpha")
            .RuleFor(i => i.RecipeId, _ => fakeRecipeOne.Id)            
            .Generate());
        var fakeIngredientTwo = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.Measure, _ => "bravo")
            .RuleFor(i => i.RecipeId, _ => fakeRecipeTwo.Id)            
            .Generate());
        var queryParameters = new IngredientParametersDto() { SortOrder = "-Measure" };

        await InsertAsync(fakeIngredientOne, fakeIngredientTwo);

        //Act
        var query = new GetIngredientList.IngredientListQuery(queryParameters);
        var ingredients = await SendAsync(query);

        // Assert
        ingredients
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeIngredientTwo, options =>
                options.ExcludingMissingMembers());
        ingredients
            .Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeIngredientOne, options =>
                options.ExcludingMissingMembers());
    }

    
    [Test]
    public async Task can_filter_ingredient_list_using_Name()
    {
        //Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    var fakeRecipeTwo = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    await InsertAsync(fakeRecipeOne, fakeRecipeTwo);

        var fakeIngredientOne = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.Name, _ => "alpha")
            .RuleFor(i => i.RecipeId, _ => fakeRecipeOne.Id)            
            .Generate());
        var fakeIngredientTwo = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.Name, _ => "bravo")
            .RuleFor(i => i.RecipeId, _ => fakeRecipeTwo.Id)            
            .Generate());
        var queryParameters = new IngredientParametersDto() { Filters = $"Name == {fakeIngredientTwo.Name}" };

        await InsertAsync(fakeIngredientOne, fakeIngredientTwo);

        //Act
        var query = new GetIngredientList.IngredientListQuery(queryParameters);
        var ingredients = await SendAsync(query);

        // Assert
        ingredients.Should().HaveCount(1);
        ingredients
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeIngredientTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_ingredient_list_using_Quantity()
    {
        //Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    var fakeRecipeTwo = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    await InsertAsync(fakeRecipeOne, fakeRecipeTwo);

        var fakeIngredientOne = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.Quantity, _ => "alpha")
            .RuleFor(i => i.RecipeId, _ => fakeRecipeOne.Id)            
            .Generate());
        var fakeIngredientTwo = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.Quantity, _ => "bravo")
            .RuleFor(i => i.RecipeId, _ => fakeRecipeTwo.Id)            
            .Generate());
        var queryParameters = new IngredientParametersDto() { Filters = $"Quantity == {fakeIngredientTwo.Quantity}" };

        await InsertAsync(fakeIngredientOne, fakeIngredientTwo);

        //Act
        var query = new GetIngredientList.IngredientListQuery(queryParameters);
        var ingredients = await SendAsync(query);

        // Assert
        ingredients.Should().HaveCount(1);
        ingredients
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeIngredientTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_ingredient_list_using_Measure()
    {
        //Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    var fakeRecipeTwo = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    await InsertAsync(fakeRecipeOne, fakeRecipeTwo);

        var fakeIngredientOne = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.Measure, _ => "alpha")
            .RuleFor(i => i.RecipeId, _ => fakeRecipeOne.Id)            
            .Generate());
        var fakeIngredientTwo = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.Measure, _ => "bravo")
            .RuleFor(i => i.RecipeId, _ => fakeRecipeTwo.Id)            
            .Generate());
        var queryParameters = new IngredientParametersDto() { Filters = $"Measure == {fakeIngredientTwo.Measure}" };

        await InsertAsync(fakeIngredientOne, fakeIngredientTwo);

        //Act
        var query = new GetIngredientList.IngredientListQuery(queryParameters);
        var ingredients = await SendAsync(query);

        // Assert
        ingredients.Should().HaveCount(1);
        ingredients
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeIngredientTwo, options =>
                options.ExcludingMissingMembers());
    }

}