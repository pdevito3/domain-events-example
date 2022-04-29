namespace RecipeManagement.UnitTests.UnitTests.Domain.RolePermissions;

using RecipeManagement.Domain;
using RecipeManagement.Domain.RolePermissions;
using RecipeManagement.Wrappers;
using SharedKernel.Dtos.RecipeManagement.RolePermission;
using SharedKernel.Domain;
using Bogus;
using FluentAssertions;
using NUnit.Framework;

public class CreateRolePermissionTests
{
    private readonly Faker _faker;

    public CreateRolePermissionTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_create_valid_rolepermission()
    {
        // Arrange
        var permission = _faker.PickRandom(Permissions.List());
        var role = _faker.PickRandom(Roles.List());

        // Act
        var newRolePermission = RolePermission.Create(new RolePermissionForCreationDto()
        {
            Permission = permission,
            Role = role
        });
        
        // Assert
        newRolePermission.Permission.Should().Be(permission);
        newRolePermission.Role.Should().Be(role);
    }
    
    [Test]
    public void can_NOT_create_rolepermission_with_invalid_role()
    {
        // Arrange
        var rolePermission = () => RolePermission.Create(new RolePermissionForCreationDto()
        {
            Permission = _faker.PickRandom(Permissions.List()),
            Role = _faker.Lorem.Word()
        });

        // Act + Assert
        rolePermission.Should().Throw<FluentValidation.ValidationException>();
    }
    
    [Test]
    public void can_NOT_create_rolepermission_with_invalid_permission()
    {
        // Arrange
        var rolePermission = () => RolePermission.Create(new RolePermissionForCreationDto()
        {
            Role = _faker.PickRandom(Roles.List()),
            Permission = _faker.Lorem.Word()
        });

        // Act + Assert
        rolePermission.Should().Throw<FluentValidation.ValidationException>();
    }
}