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

public static class AddRecipe
{
    public class AddRecipeCommand : IRequest<RecipeDto>
    {
        public RecipeForCreationDto RecipeToAdd { get; set; }

        public AddRecipeCommand(RecipeForCreationDto recipeToAdd)
        {
            RecipeToAdd = recipeToAdd;
        }
    }

    public class Handler : IRequestHandler<AddRecipeCommand, RecipeDto>
    {
        private readonly RecipesDbContext _db;
        private readonly IMapper _mapper;

        public Handler(RecipesDbContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
        }

        public async Task<RecipeDto> Handle(AddRecipeCommand request, CancellationToken cancellationToken)
        {
            var recipe = Recipe.Create(request.RecipeToAdd);
            _db.Recipes.Add(recipe);

            await _db.SaveChangesAsync(cancellationToken);

            var recipeAdded = await _db.Recipes
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == recipe.Id, cancellationToken);

            return _mapper.Map<RecipeDto>(recipeAdded);
        }
    }
}