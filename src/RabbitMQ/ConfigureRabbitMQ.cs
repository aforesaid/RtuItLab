using GreenPipes;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Consumers.Factories;
using RabbitMQ.Consumers.Identity;
using RabbitMQ.Consumers.Purchases;
using RabbitMQ.Consumers.Shops;

namespace RabbitMQ
{
    public static class ConfigureRabbitMq
    {
        public static void AddMassTransitConfigure(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                // Identity
                x.AddConsumer<Authenticate>();
                x.AddConsumer<CreateUser>();
                x.AddConsumer<GetUserByToken>();


                // Purchases
                x.AddConsumer<AddTransaction>();
                x.AddConsumer<GetTransactionById>();
                x.AddConsumer<GetTransactions>();
                x.AddConsumer<UpdateTransaction>();

                // Shops
                x.AddConsumer<BuyProducts>();
                x.AddConsumer<GetAllShops>();
                x.AddConsumer<GetProductsByCategory>();
                x.AddConsumer<GetProductsByShop>();

                // Factories
                x.AddConsumer<SubmitOrderByAddProducts>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq://guest:guest@host.docker.internal:5672");

                    cfg.ReceiveEndpoint("identityQueue", e =>
                    {
                        e.PrefetchCount = 20;
                        e.UseMessageRetry(r => r.Interval(2, 100));

                        //// Identity
                        e.Consumer<Authenticate>(context);
                        e.Consumer<CreateUser>(context);
                        e.Consumer<GetUserByToken>(context);

                    });
                    cfg.ReceiveEndpoint("purchasesQueue", e =>
                    {
                        e.PrefetchCount = 20;
                        e.UseMessageRetry(r => r.Interval(2, 100));

                        // Purchases
                        e.Consumer<AddTransaction>(context);
                        e.Consumer<GetTransactionById>(context);
                        e.Consumer<GetTransactions>(context);
                        e.Consumer<UpdateTransaction>(context);
                    });
                    cfg.ReceiveEndpoint("shopsQueue", e =>
                    {
                        e.PrefetchCount = 20;
                        e.UseMessageRetry(r => r.Interval(2, 100));

                        // Shops
                        e.Consumer<BuyProducts>(context);
                        e.Consumer<GetAllShops>(context);
                        e.Consumer<GetProductsByCategory>(context);
                        e.Consumer<GetProductsByShop>(context);
                    });
                    cfg.ReceiveEndpoint("factoriesQueue", e =>
                    {
                        e.PrefetchCount = 20;
                        e.UseMessageRetry(r => r.Interval(2, 100));

                        // Factories Consumer by Shops
                        e.Consumer<SubmitOrderByAddProducts>(context);
                        });
                    cfg.ConfigureJsonSerializer(settings =>
                    {
                        settings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;

                        return settings;
                    });
                    cfg.ConfigureJsonDeserializer(configure =>
                    {
                        configure.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
                        return configure;
                    });
                });
            });
            services.AddMassTransitHostedService();
        }
    }
}
