namespace RecipeManagement.Domain.Authors.Features;

using SharedKernel.Dtos.RecipeManagement.Author;
using SharedKernel.Exceptions;
using RecipeManagement.Databases;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

public static class GetAuthor
{
    public class AuthorQuery : IRequest<AuthorDto>
    {
        public Guid Id { get; set; }

        public AuthorQuery(Guid id)
        {
            Id = id;
        }
    }

    public class Handler : IRequestHandler<AuthorQuery, AuthorDto>
    {
        private readonly RecipesDbContext _db;
        private readonly IMapper _mapper;

        public Handler(RecipesDbContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
        }

        public async Task<AuthorDto> Handle(AuthorQuery request, CancellationToken cancellationToken)
        {
            var result = await _db.Authors
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (result == null)
                throw new NotFoundException("Author", request.Id);

            return _mapper.Map<AuthorDto>(result);
        }
    }
}