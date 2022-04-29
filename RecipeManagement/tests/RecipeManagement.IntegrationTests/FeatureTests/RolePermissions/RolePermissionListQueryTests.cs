namespace RecipeManagement.IntegrationTests.FeatureTests.RolePermissions;

using SharedKernel.Dtos.RecipeManagement.RolePermission;
using RecipeManagement.SharedTestHelpers.Fakes.RolePermission;
using SharedKernel.Exceptions;
using RecipeManagement.Domain.RolePermissions.Features;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;

public class RolePermissionListQueryTests : TestBase
{
    
    [Test]
    public async Task can_get_rolepermission_list()
    {
        // Arrange
        var fakeRolePermissionOne = FakeRolePermission.Generate(new FakeRolePermissionForCreationDto().Generate());
        var fakeRolePermissionTwo = FakeRolePermission.Generate(new FakeRolePermissionForCreationDto().Generate());
        var queryParameters = new RolePermissionParametersDto();

        await InsertAsync(fakeRolePermissionOne, fakeRolePermissionTwo);

        // Act
        var query = new GetRolePermissionList.RolePermissionListQuery(queryParameters);
        var rolePermissions = await SendAsync(query);

        // Assert
        rolePermissions.Should().HaveCount(2);
    }
    
    [Test]
    public async Task can_get_rolepermission_list_with_expected_page_size_and_number()
    {
        //Arrange
        var fakeRolePermissionOne = FakeRolePermission.Generate(new FakeRolePermissionForCreationDto().Generate());
        var fakeRolePermissionTwo = FakeRolePermission.Generate(new FakeRolePermissionForCreationDto().Generate());
        var fakeRolePermissionThree = FakeRolePermission.Generate(new FakeRolePermissionForCreationDto().Generate());
        var queryParameters = new RolePermissionParametersDto() { PageSize = 1, PageNumber = 2 };

        await InsertAsync(fakeRolePermissionOne, fakeRolePermissionTwo, fakeRolePermissionThree);

        //Act
        var query = new GetRolePermissionList.RolePermissionListQuery(queryParameters);
        var rolePermissions = await SendAsync(query);

        // Assert
        rolePermissions.Should().HaveCount(1);
    }
    
    
}