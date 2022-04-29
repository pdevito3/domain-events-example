namespace RecipeManagement.Domain.RolePermissions.Mappings;

using SharedKernel.Dtos.RecipeManagement.RolePermission;
using AutoMapper;
using RecipeManagement.Domain.RolePermissions;

public class RolePermissionProfile : Profile
{
    public RolePermissionProfile()
    {
        //createmap<to this, from this>
        CreateMap<RolePermission, RolePermissionDto>()
            .ReverseMap();
        CreateMap<RolePermissionForCreationDto, RolePermission>();
        CreateMap<RolePermissionForUpdateDto, RolePermission>()
            .ReverseMap();
    }
}