namespace RecipeManagement.Domain.Authors;

using SharedKernel.Dtos.RecipeManagement.Author;
using RecipeManagement.Domain.Authors.Mappings;
using RecipeManagement.Domain.Authors.Validators;
using RecipeManagement.Domain.Authors.DomainEvents;
using AutoMapper;
using FluentValidation;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Sieve.Attributes;
using RecipeManagement.Domain.Recipes;


public class Author : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public string Name { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    [ForeignKey("Recipe")]
    public Guid RecipeId { get; private set; }
    public Recipe Recipe { get; private set; }


    public static Author Create(AuthorForCreationDto authorForCreationDto)
    {
        new AuthorForCreationDtoValidator().ValidateAndThrow(authorForCreationDto);
        var mapper = new Mapper(new MapperConfiguration(cfg => {
            cfg.AddProfile<AuthorProfile>();
        }));
        var newAuthor = mapper.Map<Author>(authorForCreationDto);
        newAuthor.QueueDomainEvent(new AuthorCreated(){ Author = newAuthor });
        
        return newAuthor;
    }
        
    public void Update(AuthorForUpdateDto authorForUpdateDto)
    {
        new AuthorForUpdateDtoValidator().ValidateAndThrow(authorForUpdateDto);
        var mapper = new Mapper(new MapperConfiguration(cfg => {
            cfg.AddProfile<AuthorProfile>();
        }));
        mapper.Map(authorForUpdateDto, this);
        QueueDomainEvent(new AuthorUpdated(){ Author = this });
    }
    
    private Author() { } // For EF
}