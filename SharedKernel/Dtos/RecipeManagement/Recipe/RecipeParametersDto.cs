namespace SharedKernel.Dtos.RecipeManagement.Recipe
{
    using SharedKernel.Dtos.Shared;

    public class RecipeParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}