namespace SharedKernel.Dtos.RecipeManagement.RolePermission
{
    using System.Collections.Generic;
    using System;

    public abstract class RolePermissionForManipulationDto 
    {
            public string Role { get; set; }
        public string Permission { get; set; }
    }
}