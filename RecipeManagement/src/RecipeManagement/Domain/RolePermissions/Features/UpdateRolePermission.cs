namespace RecipeManagement.Domain.RolePermissions.Features;

using RecipeManagement.Domain.RolePermissions;
using SharedKernel.Dtos.RecipeManagement.RolePermission;
using SharedKernel.Exceptions;
using RecipeManagement.Databases;
using RecipeManagement.Domain.RolePermissions.Validators;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

public static class UpdateRolePermission
{
    public class UpdateRolePermissionCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public RolePermissionForUpdateDto RolePermissionToUpdate { get; set; }

        public UpdateRolePermissionCommand(Guid rolePermission, RolePermissionForUpdateDto newRolePermissionData)
        {
            Id = rolePermission;
            RolePermissionToUpdate = newRolePermissionData;
        }
    }

    public class Handler : IRequestHandler<UpdateRolePermissionCommand, bool>
    {
        private readonly RecipesDbContext _db;
        private readonly IMapper _mapper;

        public Handler(RecipesDbContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
        }

        public async Task<bool> Handle(UpdateRolePermissionCommand request, CancellationToken cancellationToken)
        {
            var rolePermissionToUpdate = await _db.RolePermissions
                .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

            if (rolePermissionToUpdate == null)
                throw new NotFoundException("RolePermission", request.Id);

            rolePermissionToUpdate.Update(request.RolePermissionToUpdate);
            await _db.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}