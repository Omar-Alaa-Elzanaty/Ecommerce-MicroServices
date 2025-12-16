using AuthenticationApi.Application.Interfaces;
using AuthenticationApi.Infrastructure.Data;
using AuthenticationApi.Infrastructure.Repositories;
using eCommerce.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IConfiguration config)
        {
            services.AddSharedServices<AuthenticationDbContext>(config, config["MySerilog:FileName"]!);

            services
                .AddScoped<IUser, UserRepository>();

            return services;
        }


        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            app.UseSharedPolices();

            return app;
        }
    }
}
