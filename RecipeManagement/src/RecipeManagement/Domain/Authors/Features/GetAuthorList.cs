namespace RecipeManagement.Domain.Authors.Features;

using RecipeManagement.Domain.Authors;
using SharedKernel.Dtos.RecipeManagement.Author;
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

public static class GetAuthorList
{
    public class AuthorListQuery : IRequest<PagedList<AuthorDto>>
    {
        public AuthorParametersDto QueryParameters { get; set; }

        public AuthorListQuery(AuthorParametersDto queryParameters)
        {
            QueryParameters = queryParameters;
        }
    }

    public class Handler : IRequestHandler<AuthorListQuery, PagedList<AuthorDto>>
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

        public async Task<PagedList<AuthorDto>> Handle(AuthorListQuery request, CancellationToken cancellationToken)
        {
            var collection = _db.Authors
                as IQueryable<Author>;

            var sieveModel = new SieveModel
            {
                Sorts = request.QueryParameters.SortOrder ?? "Id",
                Filters = request.QueryParameters.Filters
            };

            var appliedCollection = _sieveProcessor.Apply(sieveModel, collection);
            var dtoCollection = appliedCollection
                .ProjectTo<AuthorDto>(_mapper.ConfigurationProvider);

            return await PagedList<AuthorDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}