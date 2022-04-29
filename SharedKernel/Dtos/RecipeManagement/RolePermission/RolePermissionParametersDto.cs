namespace SharedKernel.Dtos.RecipeManagement.RolePermission
{
    using SharedKernel.Dtos.Shared;

    public class RolePermissionParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}