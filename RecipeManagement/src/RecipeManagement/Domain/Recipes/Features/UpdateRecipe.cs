namespace RecipeManagement.Domain.Recipes.Features;

using RecipeManagement.Domain.Recipes;
using SharedKernel.Dtos.RecipeManagement.Recipe;
using SharedKernel.Exceptions;
using RecipeManagement.Databases;
using RecipeManagement.Domain.Recipes.Validators;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

public static class UpdateRecipe
{
    public class UpdateRecipeCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public RecipeForUpdateDto RecipeToUpdate { get; set; }

        public UpdateRecipeCommand(Guid recipe, RecipeForUpdateDto newRecipeData)
        {
            Id = recipe;
            RecipeToUpdate = newRecipeData;
        }
    }

    public class Handler : IRequestHandler<UpdateRecipeCommand, bool>
    {
        private readonly RecipesDbContext _db;
        private readonly IMapper _mapper;

        public Handler(RecipesDbContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
        }

        public async Task<bool> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
        {
            var recipeToUpdate = await _db.Recipes
                .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

            if (recipeToUpdate == null)
                throw new NotFoundException("Recipe", request.Id);

            recipeToUpdate.Update(request.RecipeToUpdate);
            await _db.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}