namespace RecipeManagement.Extensions.Services;

using RecipeManagement.Resources;
using MassTransit;
using RecipeManagement.Extensions.Services.ProducerRegistrations;
using RecipeManagement.Extensions.Services.ConsumerRegistrations;
using SharedKernel.Messages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

public static class MassTransitServiceExtension
{
    public static void AddMassTransitServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        if (!env.IsEnvironment(LocalConfig.IntegrationTestingEnvName) 
            && !env.IsEnvironment(LocalConfig.FunctionalTestingEnvName))
        {
            services.AddMassTransit(mt =>
            {
                mt.AddConsumers(Assembly.GetExecutingAssembly());
                mt.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(Environment.GetEnvironmentVariable("RMQ_HOST"), Environment.GetEnvironmentVariable("RMQ_VIRTUAL_HOST"), h =>
                    {
                        h.Username(Environment.GetEnvironmentVariable("RMQ_USERNAME"));
                        h.Password(Environment.GetEnvironmentVariable("AUTH_PASSWORD"));
                    });

                    // Producers -- Do Not Delete This Comment
                    cfg.AddRecipeProducerEndpoint();

                    // Consumers -- Do Not Delete This Comment
                    cfg.AddToBookEndpoint(context);
                });
            });
            services.AddOptions<MassTransitHostOptions>();
        }
    }
}
