namespace RecipeManagement.Domain.RolePermissions.Features;

using RecipeManagement.Domain.RolePermissions;
using SharedKernel.Dtos.RecipeManagement.RolePermission;
using SharedKernel.Exceptions;
using RecipeManagement.Databases;
using RecipeManagement.Wrappers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Sieve.Models;
using Sieve.Services;
using System.Threading;
using System.Threading.Tasks;

public static class GetRolePermissionList
{
    public class RolePermissionListQuery : IRequest<PagedList<RolePermissionDto>>
    {
        public RolePermissionParametersDto QueryParameters { get; set; }

        public RolePermissionListQuery(RolePermissionParametersDto queryParameters)
        {
            QueryParameters = queryParameters;
        }
    }

    public class Handler : IRequestHandler<RolePermissionListQuery, PagedList<RolePermissionDto>>
    {
        private readonly RecipesDbContext _db;
        private readonly SieveProcessor _sieveProcessor;
        private readonly IMapper _mapper;

        public Handler(RecipesDbContext db, IMapper mapper, SieveProcessor sieveProcessor)
        {
            _mapper = mapper;
            _db = db;
            _sieveProcessor = sieveProcessor;
        }

        public async Task<PagedList<RolePermissionDto>> Handle(RolePermissionListQuery request, CancellationToken cancellationToken)
        {
            var collection = _db.RolePermissions
                as IQueryable<RolePermission>;

            var sieveModel = new SieveModel
            {
                Sorts = request.QueryParameters.SortOrder ?? "Id",
                Filters = request.QueryParameters.Filters
            };

            var appliedCollection = _sieveProcessor.Apply(sieveModel, collection);
            var dtoCollection = appliedCollection
                .ProjectTo<RolePermissionDto>(_mapper.ConfigurationProvider);

            return await PagedList<RolePermissionDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}