namespace RecipeManagement.Domain.RolePermissions.DomainEvents;

public class RolePermissionUpdated : IDomainEvent
{
    public RolePermission RolePermission { get; set; } 
}
            