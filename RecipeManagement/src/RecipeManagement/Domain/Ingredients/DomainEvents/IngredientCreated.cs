namespace RecipeManagement.Domain.Ingredients.DomainEvents;

public class IngredientCreated : IDomainEvent
{
    public Ingredient Ingredient { get; set; } 
}
            