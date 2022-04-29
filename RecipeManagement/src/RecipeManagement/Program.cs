namespace RecipeManagement;

using Serilog;
using System.Reflection;
using System.Threading.Tasks;
using RecipeManagement.Extensions.Host;

public class Program
{
    public async static Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        host.AddLoggingConfiguration();

        try
        {
            Log.Information("Starting application");
            await host.RunAsync();
        }
        catch (Exception e)
        {
            Log.Error(e, "The application failed to start correctly");
            throw;
        }
        finally
        {
            Log.Information("Shutting down application");
            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup(typeof(Startup).GetTypeInfo().Assembly.FullName)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel();
            });
}