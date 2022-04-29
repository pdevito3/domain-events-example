namespace RecipeManagement.Domain.RolePermissions.DomainEvents;

public class RolePermissionCreated : IDomainEvent
{
    public RolePermission RolePermission { get; set; } 
}
            