namespace RecipeManagement.FunctionalTests.TestUtilities;
public class ApiRoutes
{
    public const string Base = "api";
    public const string Health = Base + "/health";

    // new api route marker - do not delete

public static class Ingredients
    {
        public const string Id = "{id}";
        public const string GetList = $"{Base}/ingredients";
        public const string GetRecord = $"{Base}/ingredients/{Id}";
        public const string Create = $"{Base}/ingredients";
        public const string Delete = $"{Base}/ingredients/{Id}";
        public const string Put = $"{Base}/ingredients/{Id}";
        public const string Patch = $"{Base}/ingredients/{Id}";
        public const string CreateBatch = $"{Base}/ingredients/batch";
    }

public static class Authors
    {
        public const string Id = "{id}";
        public const string GetList = $"{Base}/authors";
        public const string GetRecord = $"{Base}/authors/{Id}";
        public const string Create = $"{Base}/authors";
        public const string Delete = $"{Base}/authors/{Id}";
        public const string Put = $"{Base}/authors/{Id}";
        public const string Patch = $"{Base}/authors/{Id}";
        public const string CreateBatch = $"{Base}/authors/batch";
    }

public static class Recipes
    {
        public const string Id = "{id}";
        public const string GetList = $"{Base}/recipes";
        public const string GetRecord = $"{Base}/recipes/{Id}";
        public const string Create = $"{Base}/recipes";
        public const string Delete = $"{Base}/recipes/{Id}";
        public const string Put = $"{Base}/recipes/{Id}";
        public const string Patch = $"{Base}/recipes/{Id}";
        public const string CreateBatch = $"{Base}/recipes/batch";
    }

public static class RolePermissions
    {
        public const string Id = "{id}";
        public const string GetList = $"{Base}/rolePermissions";
        public const string GetRecord = $"{Base}/rolePermissions/{Id}";
        public const string Create = $"{Base}/rolePermissions";
        public const string Delete = $"{Base}/rolePermissions/{Id}";
        public const string Put = $"{Base}/rolePermissions/{Id}";
        public const string Patch = $"{Base}/rolePermissions/{Id}";
        public const string CreateBatch = $"{Base}/rolePermissions/batch";
    }
}
