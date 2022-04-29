namespace RecipeManagement.Domain.Recipes.DomainEvents;

public class RecipeCreated : IDomainEvent
{
    public Recipe Recipe { get; set; } 
}
            