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

public static class AddRolePermission
{
    public class AddRolePermissionCommand : IRequest<RolePermissionDto>
    {
        public RolePermissionForCreationDto RolePermissionToAdd { get; set; }

        public AddRolePermissionCommand(RolePermissionForCreationDto rolePermissionToAdd)
        {
            RolePermissionToAdd = rolePermissionToAdd;
        }
    }

    public class Handler : IRequestHandler<AddRolePermissionCommand, RolePermissionDto>
    {
        private readonly RecipesDbContext _db;
        private readonly IMapper _mapper;

        public Handler(RecipesDbContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
        }

        public async Task<RolePermissionDto> Handle(AddRolePermissionCommand request, CancellationToken cancellationToken)
        {
            var rolePermission = RolePermission.Create(request.RolePermissionToAdd);
            _db.RolePermissions.Add(rolePermission);

            await _db.SaveChangesAsync(cancellationToken);

            var rolePermissionAdded = await _db.RolePermissions
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == rolePermission.Id, cancellationToken);

            return _mapper.Map<RolePermissionDto>(rolePermissionAdded);
        }
    }
}