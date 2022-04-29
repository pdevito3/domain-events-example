namespace RecipeManagement.Domain.Authors.DomainEvents;

public class AuthorCreated : IDomainEvent
{
    public Author Author { get; set; } 
}
            