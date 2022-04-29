namespace RecipeManagement.Domain.RolePermissions.Features;

using SharedKernel.Dtos.RecipeManagement.RolePermission;
using SharedKernel.Exceptions;
using RecipeManagement.Databases;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

public static class GetRolePermission
{
    public class RolePermissionQuery : IRequest<RolePermissionDto>
    {
        public Guid Id { get; set; }

        public RolePermissionQuery(Guid id)
        {
            Id = id;
        }
    }

    public class Handler : IRequestHandler<RolePermissionQuery, RolePermissionDto>
    {
        private readonly RecipesDbContext _db;
        private readonly IMapper _mapper;

        public Handler(RecipesDbContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
        }

        public async Task<RolePermissionDto> Handle(RolePermissionQuery request, CancellationToken cancellationToken)
        {
            var result = await _db.RolePermissions
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

            if (result == null)
                throw new NotFoundException("RolePermission", request.Id);

            return _mapper.Map<RolePermissionDto>(result);
        }
    }
}