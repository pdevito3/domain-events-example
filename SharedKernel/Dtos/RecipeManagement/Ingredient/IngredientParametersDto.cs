namespace SharedKernel.Dtos.RecipeManagement.Ingredient
{
    using SharedKernel.Dtos.Shared;

    public class IngredientParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}