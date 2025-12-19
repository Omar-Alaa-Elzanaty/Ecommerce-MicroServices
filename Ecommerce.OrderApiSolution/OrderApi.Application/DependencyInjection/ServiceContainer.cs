using eCommerce.SharedLibrary.LogsException;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Application.Services;
using Polly;
using Polly.Retry;

namespace OrderApi.Application.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services,IConfiguration config)
        {
            services.AddHttpClient<IOrderService, OrderService>(o =>
            {
                o.DefaultRequestHeaders.Add("Api-Gateway","y");
            });

            var retryStrategy = new RetryStrategyOptions()
            {
                ShouldHandle = new PredicateBuilder().Handle<TaskCanceledException>(),
                BackoffType = DelayBackoffType.Constant,
                UseJitter = false,
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromMilliseconds(500),
                OnRetry = args =>
                {
                    string message = $"OnRetry, Attempt:{args.AttemptNumber} outcome {args.Outcome}";

                    LogException.LogToConsole(message);
                    LogException.LogToDebugger(message);

                    return ValueTask.CompletedTask;
                }
            };

            services.AddResiliencePipeline("my-retry-pipeline", builder =>
            {
                builder.AddRetry(retryStrategy);
            });

            return services;
        }
    }
}
