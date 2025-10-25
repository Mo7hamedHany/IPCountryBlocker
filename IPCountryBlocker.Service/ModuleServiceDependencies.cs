using Hangfire;
using Hangfire.Redis.StackExchange;
using IPCountryBlocker.Service.Abstractions;
using IPCountryBlocker.Service.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace IPCountryBlocker.Service
{
    public static class ModuleServiceDependencies
    {
        public static IServiceCollection AddServiceDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<ILoggingService, LoggingService>();
            services.AddHttpClient<IIpGeolocationService, IpGeolocationService>()
                .ConfigureHttpClient(client =>
                {
                    client.Timeout = TimeSpan.FromSeconds(10);
                });

            var redisConnection = configuration.GetConnectionString("RedisConnection");

            var redis = ConnectionMultiplexer.Connect(redisConnection!);

            services.AddSingleton<IConnectionMultiplexer>(redis);

            services.AddHangfire(config =>
            {
                config.UseRedisStorage(redis);
            });

            services.AddHangfireServer();

            return services;
        }
    }
}
