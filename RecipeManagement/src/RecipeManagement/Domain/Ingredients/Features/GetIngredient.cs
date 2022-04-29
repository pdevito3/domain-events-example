namespace RecipeManagement.Domain.Ingredients.Features;

using SharedKernel.Dtos.RecipeManagement.Ingredient;
using SharedKernel.Exceptions;
using RecipeManagement.Databases;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

public static class GetIngredient
{
    public class IngredientQuery : IRequest<IngredientDto>
    {
        public Guid Id { get; set; }

        public IngredientQuery(Guid id)
        {
            Id = id;
        }
    }

    public class Handler : IRequestHandler<IngredientQuery, IngredientDto>
    {
        private readonly RecipesDbContext _db;
        private readonly IMapper _mapper;

        public Handler(RecipesDbContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
        }

        public async Task<IngredientDto> Handle(IngredientQuery request, CancellationToken cancellationToken)
        {
            var result = await _db.Ingredients
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (result == null)
                throw new NotFoundException("Ingredient", request.Id);

            return _mapper.Map<IngredientDto>(result);
        }
    }
}