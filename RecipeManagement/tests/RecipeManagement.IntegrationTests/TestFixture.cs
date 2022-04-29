namespace RecipeManagement.IntegrationTests;

using RecipeManagement.Databases;
using MassTransit.Testing;
using MassTransit;
using RecipeManagement.Domain.Recipes.Features;
using RecipeManagement.IntegrationTests.TestUtilities;
using RecipeManagement;
using RecipeManagement.Resources;
using RecipeManagement.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Npgsql;
using NUnit.Framework;
using Respawn;
using Respawn.Graph;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

[SetUpFixture]
public class TestFixture
{
    private static IConfigurationRoot _configuration;
    private static IWebHostEnvironment _env;
    private static IServiceScopeFactory _scopeFactory;
    private static Checkpoint _checkpoint;
    private static ServiceProvider _provider;
    private static InMemoryTestHarness _harness;

    [OneTimeSetUp]
    public async Task RunBeforeAnyTests()
    {
        var dockerDbPort = await DockerDatabaseUtilities.EnsureDockerStartedAndGetPortPortAsync();
        var dockerConnectionString = DockerDatabaseUtilities.GetSqlConnectionString(dockerDbPort.ToString());
        Environment.SetEnvironmentVariable("DB_CONNECTION_STRING", dockerConnectionString);

        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddInMemoryCollection(new Dictionary<string, string> { })
            .AddEnvironmentVariables();

        _configuration = builder.Build();
        _env = Mock.Of<IWebHostEnvironment>(e => e.EnvironmentName == LocalConfig.IntegrationTestingEnvName);

        var startup = new Startup(_configuration, _env);

        var services = new ServiceCollection();

        services.AddLogging();

        startup.ConfigureServices(services);

        // add any mock services here
        var httpContextAccessorService = services.FirstOrDefault(d =>
            d.ServiceType == typeof(IHttpContextAccessor));
        services.Remove(httpContextAccessorService);
        services.AddSingleton(_ => Mock.Of<IHttpContextAccessor>());

        // MassTransit Harness Setup -- Do Not Delete Comment
        services.AddMassTransitInMemoryTestHarness(cfg =>
        {
            // Consumer Registration -- Do Not Delete Comment

            cfg.AddConsumer<AddToBook>();
            cfg.AddConsumerTestHarness<AddToBook>();
        });

        _provider = services.BuildServiceProvider();
        _scopeFactory = _provider.GetService<IServiceScopeFactory>();

        // MassTransit Start Setup -- Do Not Delete Comment
        _harness = _provider.GetRequiredService<InMemoryTestHarness>();
        await _harness.Start();

        _checkpoint = new Checkpoint
        {
            TablesToIgnore = new Table[] { "__EFMigrationsHistory" },
            SchemasToExclude = new[] { "information_schema", "pg_subscription", "pg_catalog", "pg_toast" },
            DbAdapter = DbAdapter.Postgres
        };

        EnsureDatabase();
    }

    private static void EnsureDatabase()
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<RecipesDbContext>();

        context.Database.Migrate();
    }

    public static TScopedService GetService<TScopedService>()
    {
        var scope = _scopeFactory.CreateScope();
        var service = scope.ServiceProvider.GetService<TScopedService>();
        return service;
    }


    public static void SetUserRole(string role, string sub = null)
    {
        sub ??= Guid.NewGuid().ToString();
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.Name, sub)
        };

        var identity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);

        var httpContext = Mock.Of<HttpContext>(c => c.User == claimsPrincipal);

        var httpContextAccessor = GetService<IHttpContextAccessor>();
        httpContextAccessor.HttpContext = httpContext;
    }

    public static void SetUserRoles(string[] roles, string sub = null)
    {
        sub ??= Guid.NewGuid().ToString();
        var claims = new List<Claim>();
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        claims.Add(new Claim(ClaimTypes.Name, sub));

        var identity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);

        var httpContext = Mock.Of<HttpContext>(c => c.User == claimsPrincipal);

        var httpContextAccessor = GetService<IHttpContextAccessor>();
        httpContextAccessor.HttpContext = httpContext;
    }

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetService<ISender>();

        return await mediator.Send(request);
    }

    public static async Task ResetState()
    {
        using var conn = new NpgsqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"));
        await conn.OpenAsync();
        await _checkpoint.Reset(conn);
    }

    public static async Task<TEntity> FindAsync<TEntity>(params object[] keyValues)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<RecipesDbContext>();

        return await context.FindAsync<TEntity>(keyValues);
    }

    public static async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<RecipesDbContext>();

        context.Add(entity);

        await context.SaveChangesAsync();
    }

    public static async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<RecipesDbContext>();

        try
        {
            //await dbContext.BeginTransactionAsync();

            await action(scope.ServiceProvider);

            //await dbContext.CommitTransactionAsync();
        }
        catch (Exception)
        {
            //dbContext.RollbackTransaction();
            throw;
        }
    }

    public static async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<RecipesDbContext>();

        try
        {
            //await dbContext.BeginTransactionAsync();

            var result = await action(scope.ServiceProvider);

            //await dbContext.CommitTransactionAsync();

            return result;
        }
        catch (Exception)
        {
            //dbContext.RollbackTransaction();
            throw;
        }
    }

    public static Task ExecuteDbContextAsync(Func<RecipesDbContext, Task> action)
        => ExecuteScopeAsync(sp => action(sp.GetService<RecipesDbContext>()));

    public static Task ExecuteDbContextAsync(Func<RecipesDbContext, ValueTask> action)
        => ExecuteScopeAsync(sp => action(sp.GetService<RecipesDbContext>()).AsTask());

    public static Task ExecuteDbContextAsync(Func<RecipesDbContext, IMediator, Task> action)
        => ExecuteScopeAsync(sp => action(sp.GetService<RecipesDbContext>(), sp.GetService<IMediator>()));

    public static Task<T> ExecuteDbContextAsync<T>(Func<RecipesDbContext, Task<T>> action)
        => ExecuteScopeAsync(sp => action(sp.GetService<RecipesDbContext>()));

    public static Task<T> ExecuteDbContextAsync<T>(Func<RecipesDbContext, ValueTask<T>> action)
        => ExecuteScopeAsync(sp => action(sp.GetService<RecipesDbContext>()).AsTask());

    public static Task<T> ExecuteDbContextAsync<T>(Func<RecipesDbContext, IMediator, Task<T>> action)
        => ExecuteScopeAsync(sp => action(sp.GetService<RecipesDbContext>(), sp.GetService<IMediator>()));

    public static Task<int> InsertAsync<T>(params T[] entities) where T : class
    {
        return ExecuteDbContextAsync(db =>
        {
            foreach (var entity in entities)
            {
                db.Set<T>().Add(entity);
            }
            return db.SaveChangesAsync();
        });
    }

    // MassTransit Methods -- Do Not Delete Comment
    /// <summary>
    /// Publishes a message to the bus, and waits for the specified response.
    /// </summary>
    /// <param name="message">The message that should be published.</param>
    /// <typeparam name="TMessage">The message that should be published.</typeparam>
    public static async Task PublishMessage<TMessage>(object message)
        where TMessage : class
    {
        await _harness.Bus.Publish<TMessage>(message);
    }
    
    /// <summary>
    /// Confirm if there was a fault when publishing for this harness.
    /// </summary>
    /// <typeparam name="TMessage">The message that should be published.</typeparam>
    /// <returns>A boolean of true if there was a fault for a message of the given type when published.</returns>
    public static async Task<bool> IsFaultyPublished<TMessage>()
        where TMessage : class
    {
        return await _harness.Published.Any<Fault<TMessage>>();
    }
    
    /// <summary>
    /// Confirm that a message has been published for this harness.
    /// </summary>
    /// <typeparam name="TMessage">The message that should be published.</typeparam>
    /// <returns>A boolean of true if a message of the given type has been published.</returns>
    public static async Task<bool> IsPublished<TMessage>()
        where TMessage : class
    {
        return await _harness.Published.Any<TMessage>();
    }
    
    /// <summary>
    /// Confirm that a message has been consumed for this harness.
    /// </summary>
    /// <typeparam name="TMessage">The message that should be consumed.</typeparam>
    /// <returns>A boolean of true if a message of the given type has been consumed.</returns>
    public static async Task<bool> IsConsumed<TMessage>()
        where TMessage : class
    {
        return await _harness.Consumed.Any<TMessage>();
    }
    
    /// <summary>
    /// The desired consumer consumed the message.
    /// </summary>
    /// <typeparam name="TMessage">The message that should be consumed.</typeparam>
    /// <typeparam name="TConsumedBy">The consumer of the message.</typeparam>
    /// <returns>A boolean of true if a message of the given type has been consumed by the given consumer.</returns>
    public static async Task<bool> IsConsumed<TMessage, TConsumedBy>()
        where TMessage : class
        where TConsumedBy : class, IConsumer
    {
        var consumerHarness = _provider.GetRequiredService<IConsumerTestHarness<TConsumedBy>>();
        return await consumerHarness.Consumed.Any<TMessage>();
    }

    [OneTimeTearDown]
    public async Task RunAfterAnyTests()
    {
        // MassTransit Teardown -- Do Not Delete Comment
        await _harness.Stop();
    }
}
