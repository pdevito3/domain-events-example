namespace RecipeManagement.FunctionalTests.FunctionalTests.RolePermissions;

using RecipeManagement.SharedTestHelpers.Fakes.RolePermission;
using RecipeManagement.FunctionalTests.TestUtilities;
using RecipeManagement.Domain;
using SharedKernel.Domain;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class GetRolePermissionListTests : TestBase
{
    [Test]
    public async Task get_rolepermission_list_returns_success_using_valid_auth_credentials()
    {
        // Arrange
        

        _client.AddAuth(new[] {Roles.SuperAdmin});

        // Act
        var result = await _client.GetRequestAsync(ApiRoutes.RolePermissions.GetList);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
            
    [Test]
    public async Task get_rolepermission_list_returns_unauthorized_without_valid_token()
    {
        // Arrange
        // N/A

        // Act
        var result = await _client.GetRequestAsync(ApiRoutes.RolePermissions.GetList);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task get_rolepermission_list_returns_forbidden_without_proper_scope()
    {
        // Arrange
        _client.AddAuth();

        // Act
        var result = await _client.GetRequestAsync(ApiRoutes.RolePermissions.GetList);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}