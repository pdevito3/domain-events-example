namespace RecipeManagement.Domain.Authors.Validators;

using SharedKernel.Dtos.RecipeManagement.Author;
using FluentValidation;

public class AuthorForCreationDtoValidator: AuthorForManipulationDtoValidator<AuthorForCreationDto>
{
    public AuthorForCreationDtoValidator()
    {
        // add fluent validation rules that should only be run on creation operations here
        //https://fluentvalidation.net/
    }
}