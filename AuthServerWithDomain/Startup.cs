//This project uses Duende Software and parts of their templates (no affiliation with Wrapt or Craftsman).
//Please see the Duende Licensing information at https://duendesoftware.com/
namespace AuthServerWithDomain;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Builder;
using AuthServerWithDomain.Seeders;

public class Startup
{
    public IConfiguration _config { get; }
    public IWebHostEnvironment _env { get; }

    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        _config = configuration;
        _env = env;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();

        var identityServerBuilder = services.AddIdentityServer(options =>
        {
            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseInformationEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseSuccessEvents = true;

            // see https://docs.duendesoftware.com/identityserver/v5/fundamentals/resources/
            options.EmitStaticAudienceClaim = true;
        });

        if(_env.IsDevelopment())
        {
            identityServerBuilder.AddTestUsers(TestUsers.Users);
            identityServerBuilder.AddInMemoryIdentityResources(Config.IdentityResources);
            identityServerBuilder.AddInMemoryApiScopes(Config.ApiScopes);
            identityServerBuilder.AddInMemoryApiResources(Config.ApiResources); // this is the new api resource registration
            identityServerBuilder.AddInMemoryClients(Config.Clients);
        }

        services.AddAuthentication();
    }

    public void Configure(IApplicationBuilder app)
    {
        if (_env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();

        app.UseRouting();
        app.UseIdentityServer();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
        });
    }
}