namespace RecipeManagement.IntegrationTests.FeatureTests.RolePermissions;

using RecipeManagement.SharedTestHelpers.Fakes.RolePermission;
using RecipeManagement.IntegrationTests.TestUtilities;
using RecipeManagement.Domain.RolePermissions.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using static TestFixture;

public class DeleteRolePermissionCommandTests : TestBase
{
    [Test]
    public async Task can_delete_rolepermission_from_db()
    {
        // Arrange
        var fakeRolePermissionOne = FakeRolePermission.Generate(new FakeRolePermissionForCreationDto().Generate());
        await InsertAsync(fakeRolePermissionOne);
        var rolePermission = await ExecuteDbContextAsync(db => db.RolePermissions.SingleOrDefaultAsync());
        var id = rolePermission.Id;

        // Act
        var command = new DeleteRolePermission.DeleteRolePermissionCommand(id);
        await SendAsync(command);
        var rolePermissionResponse = await ExecuteDbContextAsync(db => db.RolePermissions.ToListAsync());

        // Assert
        rolePermissionResponse.Count.Should().Be(0);
    }

    [Test]
    public async Task delete_rolepermission_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var command = new DeleteRolePermission.DeleteRolePermissionCommand(badId);
        Func<Task> act = () => SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task can_softdelete_rolepermission_from_db()
    {
        // Arrange
        var fakeRolePermissionOne = FakeRolePermission.Generate(new FakeRolePermissionForCreationDto().Generate());
        await InsertAsync(fakeRolePermissionOne);
        var rolePermission = await ExecuteDbContextAsync(db => db.RolePermissions.SingleOrDefaultAsync());
        var id = rolePermission.Id;

        // Act
        var command = new DeleteRolePermission.DeleteRolePermissionCommand(id);
        await SendAsync(command);
        var deletedRolePermission = (await ExecuteDbContextAsync(db => db.RolePermissions
            .IgnoreQueryFilters()
            .ToListAsync())
        ).FirstOrDefault();

        // Assert
        deletedRolePermission?.IsDeleted.Should().BeTrue();
    }
}