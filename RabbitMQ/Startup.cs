using Identity.Domain.Services;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Purchases.Domain.Services;
using RabbitMQ.Consumers.Identity;
using RabbitMQ.Consumers.Purchases;
using RabbitMQ.Consumers.Shops;
using Shops.Domain.Services;
using System;
using GreenPipes;
using Newtonsoft.Json;

namespace RabbitMQ
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPurchasesService,PurchasesService>();
            services.AddScoped<IShopsService,ShopsService>();
            services.AddMassTransit(x =>
            {
                // Identity
                x.AddConsumer<Authenticate>();
                x.AddConsumer<GetUserById>();

                // Purchases
                x.AddConsumer<AddTransaction>();
                x.AddConsumer<GetTransactionById>();
                x.AddConsumer<GetTransactions>();
                x.AddConsumer<UpdateTransaction>();

                // Shops
                x.AddConsumer<AddProductsByFactory>();
                x.AddConsumer<BuyProducts>();
                x.AddConsumer<GetAllShops>();
                x.AddConsumer<GetProductsByCategory>();
                x.AddConsumer<GetProductsByShop>();

                x.UsingRabbitMq((context,cfg) =>
                    {
                        cfg.Host(new Uri("rabbitmq://localhost:5672/"), configurator =>
                        {
                            configurator.Username("guest");
                            configurator.Password("guest");
                        });

                        cfg.ReceiveEndpoint("IdentityQueue", e =>
                        {
                            e.PrefetchCount = 20;
                            e.UseMessageRetry(r => r.Interval(2, 100));

                            // Identity
                            e.Consumer<Authenticate>(context);
                            e.Consumer<GetUserById>(context);

                        });
                        cfg.ReceiveEndpoint("PurchasesQueue", e =>
                        {
                            e.PrefetchCount = 20;
                            e.UseMessageRetry(r => r.Interval(2, 100));

                            // Purchases
                            e.Consumer<AddTransaction>(context);
                            e.Consumer<GetTransactionById>(context);
                            e.Consumer<GetTransactions>(context);
                            e.Consumer<UpdateTransaction>(context);
                        });
                        cfg.ReceiveEndpoint("ShopsQueue", e =>
                        {
                            e.PrefetchCount = 20;
                            e.UseMessageRetry(r => r.Interval(2, 100));

                            // Shops
                            e.Consumer<BuyProducts>(context);
                            e.Consumer<GetAllShops>(context);
                            e.Consumer<GetProductsByCategory>(context);
                            e.Consumer<GetProductsByShop>(context);
                        });
                        cfg.ReceiveEndpoint("FactoriesQueue", e =>
                        {
                            e.PrefetchCount = 20;
                            e.UseMessageRetry(r => r.Interval(2, 100));

                            // Shops
                            e.Consumer<AddProductsByFactory>(context);
                        });
                        cfg.ConfigureJsonSerializer(settings =>
                        {
                            settings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
                            return settings;
                        });

                        cfg.ConfigureJsonDeserializer(settings =>
                        {
                            settings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
                            return settings;
                        });
                    });
            });
            services.AddMassTransitHostedService();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
