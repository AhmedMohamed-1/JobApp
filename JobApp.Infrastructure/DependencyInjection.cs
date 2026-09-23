using Hangfire;
using Hangfire.SqlServer;
using JobApp.Application.Interfaces;
using JobApp.Infrastructure.BackgroundJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JobApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
            {
                PrepareSchemaIfNecessary = true
            }));

        services.AddHangfireServer();

        services.AddScoped<CandidateNotificationJobs>();
        services.AddScoped<JobMaintenanceJobs>();
        services.AddScoped<ICandidateNotificationQueue, HangfireCandidateNotificationQueue>();

        return services;
    }
}
