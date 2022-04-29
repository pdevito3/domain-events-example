namespace RecipeManagement.Domain.Ingredients.Features;

using RecipeManagement.Domain.Ingredients;
using SharedKernel.Dtos.RecipeManagement.Ingredient;
using SharedKernel.Exceptions;
using RecipeManagement.Databases;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

public static class DeleteIngredient
{
    public class DeleteIngredientCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteIngredientCommand(Guid ingredient)
        {
            Id = ingredient;
        }
    }

    public class Handler : IRequestHandler<DeleteIngredientCommand, bool>
    {
        private readonly RecipesDbContext _db;

        public Handler(RecipesDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(DeleteIngredientCommand request, CancellationToken cancellationToken)
        {
            var recordToDelete = await _db.Ingredients
                .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (recordToDelete == null)
                throw new NotFoundException("Ingredient", request.Id);

            _db.Ingredients.Remove(recordToDelete);
            await _db.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}