namespace RecipeManagement.Domain.RolePermissions.Features;

using RecipeManagement.Domain.RolePermissions;
using SharedKernel.Dtos.RecipeManagement.RolePermission;
using SharedKernel.Exceptions;
using RecipeManagement.Databases;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

public static class DeleteRolePermission
{
    public class DeleteRolePermissionCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteRolePermissionCommand(Guid rolePermission)
        {
            Id = rolePermission;
        }
    }

    public class Handler : IRequestHandler<DeleteRolePermissionCommand, bool>
    {
        private readonly RecipesDbContext _db;

        public Handler(RecipesDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(DeleteRolePermissionCommand request, CancellationToken cancellationToken)
        {
            var recordToDelete = await _db.RolePermissions
                .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

            if (recordToDelete == null)
                throw new NotFoundException("RolePermission", request.Id);

            _db.RolePermissions.Remove(recordToDelete);
            await _db.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}