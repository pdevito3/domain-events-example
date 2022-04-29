namespace RecipeManagement.Domain.Recipes;

using SharedKernel.Dtos.RecipeManagement.Recipe;
using RecipeManagement.Domain.Recipes.Mappings;
using RecipeManagement.Domain.Recipes.Validators;
using RecipeManagement.Domain.Recipes.DomainEvents;
using AutoMapper;
using FluentValidation;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Sieve.Attributes;
using RecipeManagement.Domain.Authors;
using RecipeManagement.Domain.Ingredients;


public class Recipe : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public string Title { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public string Directions { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public int? Rating { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    public Author Author { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    public ICollection<Ingredient> Ingredients { get; private set; }


    public static Recipe Create(RecipeForCreationDto recipeForCreationDto)
    {
        new RecipeForCreationDtoValidator().ValidateAndThrow(recipeForCreationDto);
        var mapper = new Mapper(new MapperConfiguration(cfg => {
            cfg.AddProfile<RecipeProfile>();
        }));
        var newRecipe = mapper.Map<Recipe>(recipeForCreationDto);
        newRecipe.PublishDomainEvent(new RecipeCreated(){ Recipe = newRecipe });
        
        return newRecipe;
    }
        
    public void Update(RecipeForUpdateDto recipeForUpdateDto)
    {
        new RecipeForUpdateDtoValidator().ValidateAndThrow(recipeForUpdateDto);
        var mapper = new Mapper(new MapperConfiguration(cfg => {
            cfg.AddProfile<RecipeProfile>();
        }));
        mapper.Map(recipeForUpdateDto, this);
        PublishDomainEvent(new RecipeUpdated(){ Recipe = this });
    }
    
    private Recipe() { } // For EF
}