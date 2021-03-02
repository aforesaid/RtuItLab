using Factories.DAL.Data;
using Factories.Domain.Services;
using GreenPipes;
using Identity.DAL.ContextModels;
using Identity.DAL.Data;
using Identity.Domain.Services;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Purchases.DAL.Data;
using Purchases.Domain.Services;
using RabbitMQ.Consumers.Factories;
using RabbitMQ.Consumers.Identity;
using RabbitMQ.Consumers.Purchases;
using RabbitMQ.Consumers.Shops;
using Shops.DAL.Data;
using Shops.Domain.Services;

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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("identity"), ServiceLifetime.Transient);
            services.AddDbContext<PurchasesDbContext>(
                option => option.UseInMemoryDatabase("purchases"),ServiceLifetime.Transient);
            services.AddDbContext<ShopsDbContext>(
                option => option.UseInMemoryDatabase("shops"), ServiceLifetime.Transient);
            services.AddDbContext<FactoriesDbContext>(options =>
                options.UseInMemoryDatabase("factories"));
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            services.AddScoped<IPurchasesService, PurchasesService>();
            services.AddScoped<IShopsService, ShopsService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFactoriesService, FactoriesService>();
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

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
