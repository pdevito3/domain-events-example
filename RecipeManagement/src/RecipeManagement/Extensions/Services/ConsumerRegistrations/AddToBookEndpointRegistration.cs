namespace RecipeManagement.Extensions.Services.ConsumerRegistrations;

using MassTransit;
using MassTransit.RabbitMqTransport;
using RabbitMQ.Client;
using RecipeManagement.Domain.Recipes.Features;

public static class AddToBookEndpointRegistration
{
    public static void AddToBookEndpoint(this IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext context)
    {
        cfg.ReceiveEndpoint("add-recipe-to-book", re =>
        {
            // turns off default fanout settings
            re.ConfigureConsumeTopology = false;

            // a replicated queue to provide high availability and data safety. available in RMQ 3.8+
            re.SetQuorumQueue();

            // enables a lazy queue for more stable cluster with better predictive performance.
            // Please note that you should disable lazy queues if you require really high performance, if the queues are always short, or if you have set a max-length policy.
            re.SetQueueArgument("declare", "lazy");

            // the consumers that are subscribed to the endpoint
            re.ConfigureConsumer<AddToBook>(context);

            // the binding of the intermediary exchange and the primary exchange
            re.Bind("book-additions", e =>
            {
                e.ExchangeType = ExchangeType.Fanout;
            });
        });
    }
}