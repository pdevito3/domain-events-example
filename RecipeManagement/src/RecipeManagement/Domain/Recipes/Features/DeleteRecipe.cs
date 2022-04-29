namespace RecipeManagement.Domain.Recipes.Features;

using RecipeManagement.Domain.Recipes;
using SharedKernel.Dtos.RecipeManagement.Recipe;
using SharedKernel.Exceptions;
using RecipeManagement.Databases;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

public static class DeleteRecipe
{
    public class DeleteRecipeCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteRecipeCommand(Guid recipe)
        {
            Id = recipe;
        }
    }

    public class Handler : IRequestHandler<DeleteRecipeCommand, bool>
    {
        private readonly RecipesDbContext _db;

        public Handler(RecipesDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(DeleteRecipeCommand request, CancellationToken cancellationToken)
        {
            var recordToDelete = await _db.Recipes
                .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

            if (recordToDelete == null)
                throw new NotFoundException("Recipe", request.Id);

            _db.Recipes.Remove(recordToDelete);
            await _db.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}