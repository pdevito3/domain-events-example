namespace SharedKernel.Dtos.RecipeManagement.Author
{
    using SharedKernel.Dtos.Shared;

    public class AuthorParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}