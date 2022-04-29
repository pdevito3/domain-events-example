namespace RecipeManagement.UnitTests.UnitTests.Domain.RolePermissions;

using RecipeManagement.Domain;
using RecipeManagement.Domain.RolePermissions;
using RecipeManagement.Wrappers;
using SharedKernel.Dtos.RecipeManagement.RolePermission;
using SharedKernel.Domain;
using Bogus;
using FluentAssertions;
using NUnit.Framework;

public class UpdateRolePermissionTests
{
    private readonly Faker _faker;

    public UpdateRolePermissionTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_update_rolepermission()
    {
        // Arrange
        var rolePermission = RolePermission.Create(new RolePermissionForCreationDto()
        {
            Permission = _faker.PickRandom(Permissions.List()),
            Role = _faker.PickRandom(Roles.List())
        });
        var permission = _faker.PickRandom(Permissions.List());
        var role = _faker.PickRandom(Roles.List());
        
        // Act
        rolePermission.Update(new RolePermissionForUpdateDto()
        {
            Permission = permission,
            Role = role
        });
        
        // Assert
        rolePermission.Permission.Should().Be(permission);
        rolePermission.Role.Should().Be(role);
    }
    
    [Test]
    public void can_NOT_update_rolepermission_with_invalid_role()
    {
        // Arrange
        var rolePermission = RolePermission.Create(new RolePermissionForCreationDto()
        {
            Permission = _faker.PickRandom(Permissions.List()),
            Role = _faker.PickRandom(Roles.List())
        });
        var updateRolePermission = () => rolePermission.Update(new RolePermissionForUpdateDto()
        {
            Permission = _faker.PickRandom(Permissions.List()),
            Role = _faker.Lorem.Word()
        });

        // Act + Assert
        updateRolePermission.Should().Throw<FluentValidation.ValidationException>();
    }
    
    [Test]
    public void can_NOT_update_rolepermission_with_invalid_permission()
    {
        // Arrange
        var rolePermission = RolePermission.Create(new RolePermissionForCreationDto()
        {
            Permission = _faker.PickRandom(Permissions.List()),
            Role = _faker.PickRandom(Roles.List())
        });
        var updateRolePermission = () => rolePermission.Update(new RolePermissionForUpdateDto()
        {
            Permission = _faker.Lorem.Word(),
            Role = _faker.PickRandom(Roles.List())
        });

        // Act + Assert
        updateRolePermission.Should().Throw<FluentValidation.ValidationException>();
    }
}