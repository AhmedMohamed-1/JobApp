using JobApp.Application.Interfaces;
using JobApp.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace JobApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJobService, JobService>();

        return services;
    }
}
