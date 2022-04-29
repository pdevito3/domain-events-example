namespace SharedKernel.Messages
{
    using System;
    using System.Text;

    public interface IRecipeAdded
    {
            Guid RecipeId { get; set; }
    }

    // add-on property marker - Do Not Delete This Comment
}