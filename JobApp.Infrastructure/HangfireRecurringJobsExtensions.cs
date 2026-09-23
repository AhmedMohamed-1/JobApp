using Hangfire;
using JobApp.Infrastructure.BackgroundJobs;
using Microsoft.AspNetCore.Builder;

namespace JobApp.Infrastructure;

public static class HangfireRecurringJobsExtensions
{
    public static WebApplication UseHangfireRecurringJobs(this WebApplication app)
    {
        RecurringJob.AddOrUpdate<JobMaintenanceJobs>(
            "auto-close-stale-jobs",
            job => job.CloseJobsOpenTooLongAsync(),
            Cron.Daily());

        return app;
    }
}
