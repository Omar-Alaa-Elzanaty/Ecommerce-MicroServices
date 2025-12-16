using eCommerce.SharedLibrary.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Runtime.CompilerServices;

namespace eCommerce.SharedLibrary.DependencyInjection
{
    public static class SharedServiceContainer
    {
        public static IServiceCollection AddSharedServices<TContext>(
            this IServiceCollection services,
            IConfiguration config,
            string fileName)
            where TContext : DbContext
        {
            services.AddDbContext<TContext>(option => option.UseSqlServer(
                config
                .GetConnectionString("eCommerceDatabase"), sqlServerOption =>
                sqlServerOption.EnableRetryOnFailure()));

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.File(path: $"{fileName}-.text",
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {message:lj}{Exception}",
                rollingInterval: RollingInterval.Day)
                .CreateLogger();

            JWTAuthenticationScheme.AddAuthenticationScheme(services, config);
            return services;
        }

        public static IApplicationBuilder UseSharedPolices(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionHandler>();

            app.UseMiddleware<ListenToOnlyApiGateway>();

            return app;
        }
    }
}
