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

public static class AddAuthor
{
    public class AddAuthorCommand : IRequest<AuthorDto>
    {
        public AuthorForCreationDto AuthorToAdd { get; set; }

        public AddAuthorCommand(AuthorForCreationDto authorToAdd)
        {
            AuthorToAdd = authorToAdd;
        }
    }

    public class Handler : IRequestHandler<AddAuthorCommand, AuthorDto>
    {
        private readonly RecipesDbContext _db;
        private readonly IMapper _mapper;

        public Handler(RecipesDbContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
        }

        public async Task<AuthorDto> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = Author.Create(request.AuthorToAdd);
            _db.Authors.Add(author);

            await _db.SaveChangesAsync(cancellationToken);

            var authorAdded = await _db.Authors
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == author.Id, cancellationToken);

            return _mapper.Map<AuthorDto>(authorAdded);
        }
    }
}