namespace SharedKernel.Dtos.RecipeManagement.Ingredient
{
    using System.Collections.Generic;
    using System;

    public class IngredientDto 
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Quantity { get; set; }
        public string Measure { get; set; }
        public Guid RecipeId { get; set; }
    }
}