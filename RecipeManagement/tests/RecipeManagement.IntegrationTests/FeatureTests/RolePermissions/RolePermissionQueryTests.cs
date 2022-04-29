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

public class RolePermissionQueryTests : TestBase
{
    [Test]
    public async Task can_get_existing_rolepermission_with_accurate_props()
    {
        // Arrange
        var fakeRolePermissionOne = FakeRolePermission.Generate(new FakeRolePermissionForCreationDto().Generate());
        await InsertAsync(fakeRolePermissionOne);

        // Act
        var query = new GetRolePermission.RolePermissionQuery(fakeRolePermissionOne.Id);
        var rolePermissions = await SendAsync(query);

        // Assert
        rolePermissions.Should().BeEquivalentTo(fakeRolePermissionOne, options =>
            options.ExcludingMissingMembers());
    }

    [Test]
    public async Task get_rolepermission_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var query = new GetRolePermission.RolePermissionQuery(badId);
        Func<Task> act = () => SendAsync(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}