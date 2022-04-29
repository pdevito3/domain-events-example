namespace RecipeManagement.Domain.Authors.Mappings;

using SharedKernel.Dtos.RecipeManagement.Author;
using AutoMapper;
using RecipeManagement.Domain.Authors;

public class AuthorProfile : Profile
{
    public AuthorProfile()
    {
        //createmap<to this, from this>
        CreateMap<Author, AuthorDto>()
            .ReverseMap();
        CreateMap<AuthorForCreationDto, Author>();
        CreateMap<AuthorForUpdateDto, Author>()
            .ReverseMap();
    }
}