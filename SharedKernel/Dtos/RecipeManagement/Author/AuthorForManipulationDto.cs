namespace SharedKernel.Dtos.RecipeManagement.Author
{
    using System.Collections.Generic;
    using System;

    public abstract class AuthorForManipulationDto 
    {
            public string Name { get; set; }
        public Guid RecipeId { get; set; }
    }
}