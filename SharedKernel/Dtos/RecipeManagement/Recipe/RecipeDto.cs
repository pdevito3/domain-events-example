namespace SharedKernel.Dtos.RecipeManagement.Recipe
{
    using System.Collections.Generic;
    using System;

    public class RecipeDto 
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Directions { get; set; }
        public int? Rating { get; set; }

    }
}