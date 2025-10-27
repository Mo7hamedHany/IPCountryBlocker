using IPCountryBlocker.Domain.Interfaces;
using IPCountryBlocker.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace IPCountryBlocker.Infrastructure
{
    public static class ModuleInfrastructureDependencies
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton<IConnectionMultiplexer>(options =>
            {
                var config = ConfigurationOptions.Parse(configuration.GetConnectionString("RedisConnection"));

                return ConnectionMultiplexer.Connect(config);
            });

            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IBlockedAttemptRepository, BlockedAttemptRepository>();
            services.AddScoped<IAreaRepository, AreaRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IClassRepository, ClassRepository>();
            services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            services.AddScoped<IMarkRepository, MarkRepository>();

            return services;
        }
    }
}
