namespace RecipeManagement.Domain.Recipes.Features;

using RecipeManagement.Domain.Recipes;
using SharedKernel.Dtos.RecipeManagement.Recipe;
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

public static class GetRecipeList
{
    public class RecipeListQuery : IRequest<PagedList<RecipeDto>>
    {
        public RecipeParametersDto QueryParameters { get; set; }

        public RecipeListQuery(RecipeParametersDto queryParameters)
        {
            QueryParameters = queryParameters;
        }
    }

    public class Handler : IRequestHandler<RecipeListQuery, PagedList<RecipeDto>>
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

        public async Task<PagedList<RecipeDto>> Handle(RecipeListQuery request, CancellationToken cancellationToken)
        {
            var collection = _db.Recipes
                as IQueryable<Recipe>;

            var sieveModel = new SieveModel
            {
                Sorts = request.QueryParameters.SortOrder ?? "Id",
                Filters = request.QueryParameters.Filters
            };

            var appliedCollection = _sieveProcessor.Apply(sieveModel, collection);
            var dtoCollection = appliedCollection
                .ProjectTo<RecipeDto>(_mapper.ConfigurationProvider);

            return await PagedList<RecipeDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}