namespace RecipeManagement.Domain.Recipes.Features;

using SharedKernel.Dtos.RecipeManagement.Recipe;
using SharedKernel.Exceptions;
using RecipeManagement.Databases;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

public static class GetRecipe
{
    public class RecipeQuery : IRequest<RecipeDto>
    {
        public Guid Id { get; set; }

        public RecipeQuery(Guid id)
        {
            Id = id;
        }
    }

    public class Handler : IRequestHandler<RecipeQuery, RecipeDto>
    {
        private readonly RecipesDbContext _db;
        private readonly IMapper _mapper;

        public Handler(RecipesDbContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
        }

        public async Task<RecipeDto> Handle(RecipeQuery request, CancellationToken cancellationToken)
        {
            var result = await _db.Recipes
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

            if (result == null)
                throw new NotFoundException("Recipe", request.Id);

            return _mapper.Map<RecipeDto>(result);
        }
    }
}