namespace RecipeManagement.Domain.Recipes.Features;

using AutoMapper;
using MassTransit;
using SharedKernel.Messages;
using System.Threading.Tasks;
using RecipeManagement.Databases;

public class AddToBook : IConsumer<IRecipeAdded>
{
    private readonly IMapper _mapper;
    private readonly RecipesDbContext _db;

    public AddToBook(RecipesDbContext db, IMapper mapper)
    {
        _mapper = mapper;
        _db = db;
    }

    public Task Consume(ConsumeContext<IRecipeAdded> context)
    {
        // do work here

        return Task.CompletedTask;
    }
}