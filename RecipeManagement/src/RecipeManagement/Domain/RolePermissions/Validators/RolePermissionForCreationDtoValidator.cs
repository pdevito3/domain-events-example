namespace RecipeManagement.Domain.RolePermissions.Validators;

using SharedKernel.Dtos.RecipeManagement.RolePermission;
using FluentValidation;

public class RolePermissionForCreationDtoValidator: RolePermissionForManipulationDtoValidator<RolePermissionForCreationDto>
{
    public RolePermissionForCreationDtoValidator()
    {
        // add fluent validation rules that should only be run on creation operations here
        //https://fluentvalidation.net/
    }
}