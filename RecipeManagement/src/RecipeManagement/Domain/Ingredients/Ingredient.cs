namespace RecipeManagement.Domain.Ingredients;

using SharedKernel.Dtos.RecipeManagement.Ingredient;
using RecipeManagement.Domain.Ingredients.Mappings;
using RecipeManagement.Domain.Ingredients.Validators;
using RecipeManagement.Domain.Ingredients.DomainEvents;
using AutoMapper;
using FluentValidation;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Sieve.Attributes;
using RecipeManagement.Domain.Recipes;


public class Ingredient : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public string Name { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public string Quantity { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public string Measure { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    [ForeignKey("Recipe")]
    public Guid RecipeId { get; private set; }
    public Recipe Recipe { get; private set; }


    public static Ingredient Create(IngredientForCreationDto ingredientForCreationDto)
    {
        new IngredientForCreationDtoValidator().ValidateAndThrow(ingredientForCreationDto);
        var mapper = new Mapper(new MapperConfiguration(cfg => {
            cfg.AddProfile<IngredientProfile>();
        }));
        var newIngredient = mapper.Map<Ingredient>(ingredientForCreationDto);
        newIngredient.PublishDomainEvent(new IngredientCreated(){ Ingredient = newIngredient });
        
        return newIngredient;
    }
        
    public void Update(IngredientForUpdateDto ingredientForUpdateDto)
    {
        new IngredientForUpdateDtoValidator().ValidateAndThrow(ingredientForUpdateDto);
        var mapper = new Mapper(new MapperConfiguration(cfg => {
            cfg.AddProfile<IngredientProfile>();
        }));
        mapper.Map(ingredientForUpdateDto, this);
        PublishDomainEvent(new IngredientUpdated(){ Ingredient = this });
    }
    
    private Ingredient() { } // For EF
}