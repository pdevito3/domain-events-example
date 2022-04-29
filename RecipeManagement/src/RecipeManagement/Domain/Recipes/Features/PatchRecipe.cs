namespace RecipeManagement.Domain.Recipes.Features;

using RecipeManagement.Domain.Recipes;
using SharedKernel.Dtos.RecipeManagement.Recipe;
using SharedKernel.Exceptions;
using RecipeManagement.Databases;
using RecipeManagement.Domain.Recipes.Validators;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

public static class PatchRecipe
{
    public class PatchRecipeCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public JsonPatchDocument<RecipeForUpdateDto> PatchDoc { get; set; }

        public PatchRecipeCommand(Guid recipe, JsonPatchDocument<RecipeForUpdateDto> patchDoc)
        {
            Id = recipe;
            PatchDoc = patchDoc;
        }
    }

    public class Handler : IRequestHandler<PatchRecipeCommand, bool>
    {
        private readonly RecipesDbContext _db;
        private readonly IMapper _mapper;

        public Handler(RecipesDbContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
        }

        public async Task<bool> Handle(PatchRecipeCommand request, CancellationToken cancellationToken)
        {
            if (request.PatchDoc == null)
                throw new ValidationException(
                    new List<ValidationFailure>()
                    {
                        new ValidationFailure("Patch Document","Invalid patch doc.")
                    });

            var recipeToUpdate = await _db.Recipes
                .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

            if (recipeToUpdate == null)
                throw new NotFoundException("Recipe", request.Id);

            var recipeToPatch = _mapper.Map<RecipeForUpdateDto>(recipeToUpdate); // map the recipe we got from the database to an updatable recipe dto
            request.PatchDoc.ApplyTo(recipeToPatch); // apply patchdoc updates to the updatable recipe dto

            recipeToUpdate.Update(recipeToPatch);
            await _db.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}