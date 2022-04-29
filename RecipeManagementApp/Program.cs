using RecipeManagementApp.Extensions.Host;
using Duende.Bff;
using Duende.Bff.Yarp;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.AddLoggingConfiguration(builder.Environment);

builder.Services.AddControllers();
builder.Services.AddBff()
    .AddRemoteApis();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "cookie";
        options.DefaultChallengeScheme = "oidc";
        options.DefaultSignOutScheme = "oidc";
    })
    .AddCookie("cookie", options =>
    {
        options.Cookie.Name = "__Host-RecipeManagementApp-bff";
        options.Cookie.SameSite = SameSiteMode.Strict;
    })
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = Environment.GetEnvironmentVariable("AUTH_AUTHORITY");
        options.ClientId = Environment.GetEnvironmentVariable("AUTH_CLIENT_ID");
        options.ClientSecret = Environment.GetEnvironmentVariable("AUTH_CLIENT_SECRET");
        options.ResponseType = "code";
        options.ResponseMode = "query";

        options.GetClaimsFromUserInfoEndpoint = true;
        options.MapInboundClaims = false;
        options.SaveTokens = true;

        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("offline_access");
        
        // boundary scopes
        options.Scope.Add("recipe_management");

        options.TokenValidationParameters = new()
        {
            NameClaimType = "name",
            RoleClaimType = "role"
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();

// adds route matching to the middleware pipeline. This middleware looks at the set of endpoints defined in the app, and selects the best match based on the request.
app.UseRouting();

app.UseAuthentication();
app.UseBff();
app.UseAuthorization();

// adds endpoint execution to the middleware pipeline. It runs the delegate associated with the selected endpoint.
app.MapBffManagementEndpoints();

app.MapControllers()
    .RequireAuthorization()
    .AsBffApiEndpoint();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRemoteBffApiEndpoint("/api/recipes", "https://localhost:5375/api/recipes")
        .RequireAccessToken();
    endpoints.MapRemoteBffApiEndpoint("/api/ingredients", "https://localhost:5375/api/ingredients")
        .RequireAccessToken();
});

app.MapFallbackToFile("index.html");

try
{
    Log.Information("Starting application");
    await app.RunAsync();
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