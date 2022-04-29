namespace RecipeManagement.Domain.Recipes.Features;

using SharedKernel.Messages;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using RecipeManagement.Databases;

public static class AddRecipeProducer
{
    public class AddRecipeProducerCommand : IRequest<bool>
    {
        public AddRecipeProducerCommand()
        {
        }
    }

    public class Handler : IRequestHandler<AddRecipeProducerCommand, bool>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;
        private readonly RecipesDbContext _db;

        public Handler(RecipesDbContext db, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
            _db = db;
        }

        public async Task<bool> Handle(AddRecipeProducerCommand request, CancellationToken cancellationToken)
        {
            var message = new
            {
                // map content to message here or with automapper
            };
            await _publishEndpoint.Publish<IRecipeAdded>(message, cancellationToken);

            return true;
        }
    }
}