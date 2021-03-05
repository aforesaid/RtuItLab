using Factories.DAL.Data;
using Factories.Domain.Services;
using Identity.DAL.ContextModels;
using Identity.DAL.Data;
using Identity.Domain.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Purchases.DAL.Data;
using Purchases.Domain.Services;
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
                options.UseInMemoryDatabase("factories"), ServiceLifetime.Transient);
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            services.AddScoped<IPurchasesService, PurchasesService>();
            services.AddScoped<IShopsService, ShopsService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFactoriesService, FactoriesService>();
            services.AddMassTransitConfigure();
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
