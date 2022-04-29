namespace RecipeManagement.Domain.Authors.Features;

using RecipeManagement.Domain.Authors;
using SharedKernel.Dtos.RecipeManagement.Author;
using SharedKernel.Exceptions;
using RecipeManagement.Databases;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

public static class DeleteAuthor
{
    public class DeleteAuthorCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteAuthorCommand(Guid author)
        {
            Id = author;
        }
    }

    public class Handler : IRequestHandler<DeleteAuthorCommand, bool>
    {
        private readonly RecipesDbContext _db;

        public Handler(RecipesDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            var recordToDelete = await _db.Authors
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (recordToDelete == null)
                throw new NotFoundException("Author", request.Id);

            _db.Authors.Remove(recordToDelete);
            await _db.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}