namespace RecipeManagement.Domain.Ingredients.Features;

using RecipeManagement.Domain.Ingredients;
using SharedKernel.Dtos.RecipeManagement.Ingredient;
using SharedKernel.Exceptions;
using RecipeManagement.Databases;
using RecipeManagement.Domain.Ingredients.Validators;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

public static class AddIngredientList
{
    public class AddIngredientListCommand : IRequest<IEnumerable<IngredientDto>>
    {
        public IEnumerable<IngredientForCreationDto> IngredientListToAdd { get; set; }
        public Guid RecipeId { get; set; }

        public AddIngredientListCommand(IEnumerable<IngredientForCreationDto> ingredientListListToAdd, Guid recipeId)
        {
            IngredientListToAdd = ingredientListListToAdd;
            RecipeId = recipeId;
        }
    }

    public class Handler : IRequestHandler<AddIngredientListCommand, IEnumerable<IngredientDto>>
    {
        private readonly RecipesDbContext _db;
        private readonly IMapper _mapper;

        public Handler(RecipesDbContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
        }

        public async Task<IEnumerable<IngredientDto>> Handle(AddIngredientListCommand request, CancellationToken cancellationToken)
        {
            var fkEntity = await _db.Recipes.FirstOrDefaultAsync(x => x.Id == request.RecipeId, cancellationToken);
            if (fkEntity == null)
                throw new NotFoundException($"No RecipeId found with an id of '{request.RecipeId}'");

var ingredientListToAdd = request.IngredientListToAdd
                .Select(i => { i.RecipeId = request.RecipeId; return i; })
                .ToList();
            var ingredientList = new List<Ingredient>();
            ingredientListToAdd.ForEach(ingredient => ingredientList.Add(Ingredient.Create(ingredient)));
            
            _db.Ingredients.AddRangeAsync(ingredientList, cancellationToken);

            await _db.SaveChangesAsync(cancellationToken);

            var result = _db.Ingredients.Where(i => ingredientList.Select(il => il.Id).Contains(i.Id));
            return _mapper.Map<IEnumerable<IngredientDto>>(result);
        }
    }
}