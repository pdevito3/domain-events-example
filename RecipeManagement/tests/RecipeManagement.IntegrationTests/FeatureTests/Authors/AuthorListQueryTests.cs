namespace RecipeManagement.IntegrationTests.FeatureTests.Authors;

using SharedKernel.Dtos.RecipeManagement.Author;
using RecipeManagement.SharedTestHelpers.Fakes.Author;
using SharedKernel.Exceptions;
using RecipeManagement.Domain.Authors.Features;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;
using RecipeManagement.SharedTestHelpers.Fakes.Recipe;

public class AuthorListQueryTests : TestBase
{
    
    [Test]
    public async Task can_get_author_list()
    {
        // Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    var fakeRecipeTwo = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    await InsertAsync(fakeRecipeOne, fakeRecipeTwo);

        var fakeAuthorOne = FakeAuthor.Generate(new FakeAuthorForCreationDto()
            .RuleFor(a => a.RecipeId, _ => fakeRecipeOne.Id)
            .Generate());
        var fakeAuthorTwo = FakeAuthor.Generate(new FakeAuthorForCreationDto()
            .RuleFor(a => a.RecipeId, _ => fakeRecipeTwo.Id)
            .Generate());
        var queryParameters = new AuthorParametersDto();

        await InsertAsync(fakeAuthorOne, fakeAuthorTwo);

        // Act
        var query = new GetAuthorList.AuthorListQuery(queryParameters);
        var authors = await SendAsync(query);

        // Assert
        authors.Should().HaveCount(2);
    }
    
    [Test]
    public async Task can_get_author_list_with_expected_page_size_and_number()
    {
        //Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    var fakeRecipeTwo = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    var fakeRecipeThree = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    await InsertAsync(fakeRecipeOne, fakeRecipeTwo, fakeRecipeThree);

        var fakeAuthorOne = FakeAuthor.Generate(new FakeAuthorForCreationDto()
            .RuleFor(a => a.RecipeId, _ => fakeRecipeOne.Id)
            .Generate());
        var fakeAuthorTwo = FakeAuthor.Generate(new FakeAuthorForCreationDto()
            .RuleFor(a => a.RecipeId, _ => fakeRecipeTwo.Id)
            .Generate());
        var fakeAuthorThree = FakeAuthor.Generate(new FakeAuthorForCreationDto()
            .RuleFor(a => a.RecipeId, _ => fakeRecipeThree.Id)
            .Generate());
        var queryParameters = new AuthorParametersDto() { PageSize = 1, PageNumber = 2 };

        await InsertAsync(fakeAuthorOne, fakeAuthorTwo, fakeAuthorThree);

        //Act
        var query = new GetAuthorList.AuthorListQuery(queryParameters);
        var authors = await SendAsync(query);

        // Assert
        authors.Should().HaveCount(1);
    }
    
    [Test]
    public async Task can_get_sorted_list_of_author_by_Name_in_asc_order()
    {
        //Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    var fakeRecipeTwo = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    await InsertAsync(fakeRecipeOne, fakeRecipeTwo);

        var fakeAuthorOne = FakeAuthor.Generate(new FakeAuthorForCreationDto()
            .RuleFor(a => a.Name, _ => "bravo")
            .RuleFor(a => a.RecipeId, _ => fakeRecipeOne.Id)            
            .Generate());
        var fakeAuthorTwo = FakeAuthor.Generate(new FakeAuthorForCreationDto()
            .RuleFor(a => a.Name, _ => "alpha")
            .RuleFor(a => a.RecipeId, _ => fakeRecipeTwo.Id)            
            .Generate());
        var queryParameters = new AuthorParametersDto() { SortOrder = "Name" };

        await InsertAsync(fakeAuthorOne, fakeAuthorTwo);

        //Act
        var query = new GetAuthorList.AuthorListQuery(queryParameters);
        var authors = await SendAsync(query);

        // Assert
        authors
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeAuthorTwo, options =>
                options.ExcludingMissingMembers());
        authors
            .Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeAuthorOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_author_by_Name_in_desc_order()
    {
        //Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    var fakeRecipeTwo = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    await InsertAsync(fakeRecipeOne, fakeRecipeTwo);

        var fakeAuthorOne = FakeAuthor.Generate(new FakeAuthorForCreationDto()
            .RuleFor(a => a.Name, _ => "alpha")
            .RuleFor(a => a.RecipeId, _ => fakeRecipeOne.Id)            
            .Generate());
        var fakeAuthorTwo = FakeAuthor.Generate(new FakeAuthorForCreationDto()
            .RuleFor(a => a.Name, _ => "bravo")
            .RuleFor(a => a.RecipeId, _ => fakeRecipeTwo.Id)            
            .Generate());
        var queryParameters = new AuthorParametersDto() { SortOrder = "-Name" };

        await InsertAsync(fakeAuthorOne, fakeAuthorTwo);

        //Act
        var query = new GetAuthorList.AuthorListQuery(queryParameters);
        var authors = await SendAsync(query);

        // Assert
        authors
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeAuthorTwo, options =>
                options.ExcludingMissingMembers());
        authors
            .Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeAuthorOne, options =>
                options.ExcludingMissingMembers());
    }

    
    [Test]
    public async Task can_filter_author_list_using_Name()
    {
        //Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    var fakeRecipeTwo = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    await InsertAsync(fakeRecipeOne, fakeRecipeTwo);

        var fakeAuthorOne = FakeAuthor.Generate(new FakeAuthorForCreationDto()
            .RuleFor(a => a.Name, _ => "alpha")
            .RuleFor(a => a.RecipeId, _ => fakeRecipeOne.Id)            
            .Generate());
        var fakeAuthorTwo = FakeAuthor.Generate(new FakeAuthorForCreationDto()
            .RuleFor(a => a.Name, _ => "bravo")
            .RuleFor(a => a.RecipeId, _ => fakeRecipeTwo.Id)            
            .Generate());
        var queryParameters = new AuthorParametersDto() { Filters = $"Name == {fakeAuthorTwo.Name}" };

        await InsertAsync(fakeAuthorOne, fakeAuthorTwo);

        //Act
        var query = new GetAuthorList.AuthorListQuery(queryParameters);
        var authors = await SendAsync(query);

        // Assert
        authors.Should().HaveCount(1);
        authors
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeAuthorTwo, options =>
                options.ExcludingMissingMembers());
    }

}