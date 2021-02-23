using Factories.API.Data;
using Factories.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebRabbitMQ;

namespace Factories.API
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
            services.AddDbContext<FactoriesDbContext>(options =>
                                                          options.UseInMemoryDatabase("Factories"));
            services.AddScoped<IFactoriesService, FactoriesService>();
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
            var component = app.ApplicationServices.GetRequiredService<IEventBus>();
            component.Subscribe();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}