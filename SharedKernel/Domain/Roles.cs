namespace SharedKernel.Domain
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class Roles
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string User = "User";
        
        public static List<string> List()
        {
            return typeof(Roles)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
                .Select(x => (string)x.GetRawConstantValue())
                .ToList();
        }
    }
}