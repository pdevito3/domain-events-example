namespace RecipeManagement.Domain.Ingredients.DomainEvents;

public class IngredientUpdated : IDomainEvent
{
    public Ingredient Ingredient { get; set; } 
}
            