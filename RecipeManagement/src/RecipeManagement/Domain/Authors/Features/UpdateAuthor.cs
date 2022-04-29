namespace RecipeManagement.Domain.Authors.Features;

using RecipeManagement.Domain.Authors;
using SharedKernel.Dtos.RecipeManagement.Author;
using SharedKernel.Exceptions;
using RecipeManagement.Databases;
using RecipeManagement.Domain.Authors.Validators;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

public static class UpdateAuthor
{
    public class UpdateAuthorCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public AuthorForUpdateDto AuthorToUpdate { get; set; }

        public UpdateAuthorCommand(Guid author, AuthorForUpdateDto newAuthorData)
        {
            Id = author;
            AuthorToUpdate = newAuthorData;
        }
    }

    public class Handler : IRequestHandler<UpdateAuthorCommand, bool>
    {
        private readonly RecipesDbContext _db;
        private readonly IMapper _mapper;

        public Handler(RecipesDbContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
        }

        public async Task<bool> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            var authorToUpdate = await _db.Authors
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (authorToUpdate == null)
                throw new NotFoundException("Author", request.Id);

            authorToUpdate.Update(request.AuthorToUpdate);
            await _db.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}