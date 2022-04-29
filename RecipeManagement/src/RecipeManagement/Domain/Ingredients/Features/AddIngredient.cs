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

public static class AddIngredient
{
    public class AddIngredientCommand : IRequest<IngredientDto>
    {
        public IngredientForCreationDto IngredientToAdd { get; set; }

        public AddIngredientCommand(IngredientForCreationDto ingredientToAdd)
        {
            IngredientToAdd = ingredientToAdd;
        }
    }

    public class Handler : IRequestHandler<AddIngredientCommand, IngredientDto>
    {
        private readonly RecipesDbContext _db;
        private readonly IMapper _mapper;

        public Handler(RecipesDbContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
        }

        public async Task<IngredientDto> Handle(AddIngredientCommand request, CancellationToken cancellationToken)
        {
            var ingredient = Ingredient.Create(request.IngredientToAdd);
            _db.Ingredients.Add(ingredient);

            await _db.SaveChangesAsync(cancellationToken);

            var ingredientAdded = await _db.Ingredients
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == ingredient.Id, cancellationToken);

            return _mapper.Map<IngredientDto>(ingredientAdded);
        }
    }
}