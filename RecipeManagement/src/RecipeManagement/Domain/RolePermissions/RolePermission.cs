namespace RecipeManagement.Domain.RolePermissions;

using SharedKernel.Dtos.RecipeManagement.RolePermission;
using RecipeManagement.Domain.RolePermissions.Mappings;
using RecipeManagement.Domain.RolePermissions.Validators;
using RecipeManagement.Domain.RolePermissions.DomainEvents;
using AutoMapper;
using FluentValidation;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;


public class RolePermission : BaseEntity
{
    public string Role { get; private set; }

    public string Permission { get; private set; }


    public static RolePermission Create(RolePermissionForCreationDto rolePermissionForCreationDto)
    {
        new RolePermissionForCreationDtoValidator().ValidateAndThrow(rolePermissionForCreationDto);
        var mapper = new Mapper(new MapperConfiguration(cfg => {
            cfg.AddProfile<RolePermissionProfile>();
        }));
        var newRolePermission = mapper.Map<RolePermission>(rolePermissionForCreationDto);
        newRolePermission.QueueDomainEvent(new RolePermissionCreated(){ RolePermission = newRolePermission });
        
        return newRolePermission;
    }
        
    public void Update(RolePermissionForUpdateDto rolePermissionForUpdateDto)
    {
        new RolePermissionForUpdateDtoValidator().ValidateAndThrow(rolePermissionForUpdateDto);
        var mapper = new Mapper(new MapperConfiguration(cfg => {
            cfg.AddProfile<RolePermissionProfile>();
        }));
        mapper.Map(rolePermissionForUpdateDto, this);
        QueueDomainEvent(new RolePermissionUpdated(){ RolePermission = this });
    }
    
    private RolePermission() { } // For EF
}