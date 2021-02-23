using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Purchases.API.Data;
using Purchases.API.Helpers;
using Purchases.API.Services;
using WebRabbitMQ;

namespace Purchases.API
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
            services.AddControllers();
            services.AddDbContext<PurchasesDbContext>(
                                                      option => option.UseInMemoryDatabase("purchases"));
            services.AddScoped<IPurchasesService, PurchasesService>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title   = "Purchases Service",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
                    Name   = "Authorization",
                    In     = ParameterLocation.Header,
                    Type   = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id   = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name   = "Bearer",
                            In     = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });
            services.AddSingleton<IEventBus, RabbitMQBus>(s =>
            {
                var lifeTime = s.GetRequiredService<IHostApplicationLifetime>();
                return new RabbitMQBus(lifeTime);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseSwagger()
                .UseSwaggerUI(c =>
                              {
                                  c.SwaggerEndpoint("/swagger/v1/swagger.json", "Purchases.API V1");
                              }
                             );
            var component = app.ApplicationServices.GetRequiredService<IEventBus>();
            component.Subscribe();
            app.UseRouting();
            app.UseMiddleware<UserMiddleware>();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}