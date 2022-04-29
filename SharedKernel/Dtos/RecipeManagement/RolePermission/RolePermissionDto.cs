namespace SharedKernel.Dtos.RecipeManagement.RolePermission
{
    using System.Collections.Generic;
    using System;

    public class RolePermissionDto 
    {
        public Guid Id { get; set; }
        public string Role { get; set; }
        public string Permission { get; set; }
    }
}