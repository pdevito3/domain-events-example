namespace RecipeManagement.Domain.Recipes.DomainEvents;

public class RecipeUpdated : IDomainEvent
{
    public Recipe Recipe { get; set; } 
}
            