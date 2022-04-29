namespace RecipeManagement.FunctionalTests.FunctionalTests.RolePermissions;

using RecipeManagement.SharedTestHelpers.Fakes.RolePermission;
using RecipeManagement.FunctionalTests.TestUtilities;
using RecipeManagement.Domain;
using SharedKernel.Domain;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class CreateRolePermissionTests : TestBase
{
    [Test]
    public async Task create_rolepermission_returns_created_using_valid_dto_and_valid_auth_credentials()
    {
        // Arrange
        var fakeRolePermission = new FakeRolePermissionForCreationDto { }.Generate();

        _client.AddAuth(new[] {Roles.SuperAdmin});

        // Act
        var route = ApiRoutes.RolePermissions.Create;
        var result = await _client.PostJsonRequestAsync(route, fakeRolePermission);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }
            
    [Test]
    public async Task create_rolepermission_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeRolePermission = FakeRolePermission.Generate(new FakeRolePermissionForCreationDto().Generate());

        await InsertAsync(fakeRolePermission);

        // Act
        var route = ApiRoutes.RolePermissions.Create;
        var result = await _client.PostJsonRequestAsync(route, fakeRolePermission);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task create_rolepermission_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeRolePermission = FakeRolePermission.Generate(new FakeRolePermissionForCreationDto().Generate());
        _client.AddAuth();

        await InsertAsync(fakeRolePermission);

        // Act
        var route = ApiRoutes.RolePermissions.Create;
        var result = await _client.PostJsonRequestAsync(route, fakeRolePermission);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}