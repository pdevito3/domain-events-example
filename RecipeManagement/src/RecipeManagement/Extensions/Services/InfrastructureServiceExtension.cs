namespace RecipeManagement.Extensions.Services;

using RecipeManagement.Databases;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using HeimGuard;
using RecipeManagement.Services;
using RecipeManagement.Resources;
using Microsoft.EntityFrameworkCore;

public static class ServiceRegistration
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        // DbContext -- Do Not Delete
        if (env.IsEnvironment(LocalConfig.FunctionalTestingEnvName))
        {
            services.AddDbContext<RecipesDbContext>(options =>
                options.UseInMemoryDatabase($"RecipeManagement"));
        }
        else
        {
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            if(string.IsNullOrEmpty(connectionString))
            {
                // this makes local migrations easier to manage. feel free to refactor if desired.
                connectionString = env.IsDevelopment() 
                    ? "Host=localhost;Port=59989;Database=dev_recipemanagement;Username=postgres;Password=postgres"
                    : throw new Exception("DB_CONNECTION_STRING environment variable is not set.");
            }

            services.AddDbContext<RecipesDbContext>(options =>
                options.UseNpgsql(connectionString,
                    builder => builder.MigrationsAssembly(typeof(RecipesDbContext).Assembly.FullName))
                            .UseSnakeCaseNamingConvention());
        }

        // Auth -- Do Not Delete
        if (!env.IsEnvironment(LocalConfig.FunctionalTestingEnvName))
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = Environment.GetEnvironmentVariable("AUTH_AUTHORITY");
                    options.Audience = Environment.GetEnvironmentVariable("AUTH_AUDIENCE");
                });
        }

        services.AddAuthorization(options =>
        {
        });

        services.AddHeimGuard<UserPolicyHandler>()
            .MapAuthorizationPolicies()
            .AutomaticallyCheckPermissions();
    }
}
