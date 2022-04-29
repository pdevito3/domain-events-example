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

public static class UpdateIngredient
{
    public class UpdateIngredientCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public IngredientForUpdateDto IngredientToUpdate { get; set; }

        public UpdateIngredientCommand(Guid ingredient, IngredientForUpdateDto newIngredientData)
        {
            Id = ingredient;
            IngredientToUpdate = newIngredientData;
        }
    }

    public class Handler : IRequestHandler<UpdateIngredientCommand, bool>
    {
        private readonly RecipesDbContext _db;
        private readonly IMapper _mapper;

        public Handler(RecipesDbContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
        }

        public async Task<bool> Handle(UpdateIngredientCommand request, CancellationToken cancellationToken)
        {
            var ingredientToUpdate = await _db.Ingredients
                .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (ingredientToUpdate == null)
                throw new NotFoundException("Ingredient", request.Id);

            ingredientToUpdate.Update(request.IngredientToUpdate);
            await _db.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}