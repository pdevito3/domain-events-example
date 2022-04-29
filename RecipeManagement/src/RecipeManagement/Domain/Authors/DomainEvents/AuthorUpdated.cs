namespace RecipeManagement.Domain.Authors.DomainEvents;

public class AuthorUpdated : IDomainEvent
{
    public Author Author { get; set; } 
}
            