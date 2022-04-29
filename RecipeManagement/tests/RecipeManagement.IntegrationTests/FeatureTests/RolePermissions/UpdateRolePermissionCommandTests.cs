namespace RecipeManagement.IntegrationTests.FeatureTests.RolePermissions;

using RecipeManagement.SharedTestHelpers.Fakes.RolePermission;
using RecipeManagement.IntegrationTests.TestUtilities;
using SharedKernel.Dtos.RecipeManagement.RolePermission;
using SharedKernel.Exceptions;
using RecipeManagement.Domain.RolePermissions.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using static TestFixture;

public class UpdateRolePermissionCommandTests : TestBase
{
    [Test]
    public async Task can_update_existing_rolepermission_in_db()
    {
        // Arrange
        var fakeRolePermissionOne = FakeRolePermission.Generate(new FakeRolePermissionForCreationDto().Generate());
        var updatedRolePermissionDto = new FakeRolePermissionForUpdateDto().Generate();
        await InsertAsync(fakeRolePermissionOne);

        var rolePermission = await ExecuteDbContextAsync(db => db.RolePermissions.SingleOrDefaultAsync());
        var id = rolePermission.Id;

        // Act
        var command = new UpdateRolePermission.UpdateRolePermissionCommand(id, updatedRolePermissionDto);
        await SendAsync(command);
        var updatedRolePermission = await ExecuteDbContextAsync(db => db.RolePermissions.Where(r => r.Id == id).SingleOrDefaultAsync());

        // Assert
        updatedRolePermission.Should().BeEquivalentTo(updatedRolePermissionDto, options =>
            options.ExcludingMissingMembers());
    }
}