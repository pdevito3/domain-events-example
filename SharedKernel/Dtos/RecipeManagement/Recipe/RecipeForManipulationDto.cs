namespace SharedKernel.Dtos.RecipeManagement.Recipe
{
    using System.Collections.Generic;
    using System;

    public abstract class RecipeForManipulationDto 
    {
            public string Title { get; set; }
        public string Directions { get; set; }
        public int? Rating { get; set; }

    }
}