using JobApp.Application.Interfaces;
using JobApp.Domain.Enums;

namespace JobApp.Infrastructure.BackgroundJobs;

public class JobMaintenanceJobs
{
    private const int MaxOpenDays = 20;

    private readonly IJobRepository _jobRepository;

    public JobMaintenanceJobs(IJobRepository jobRepository)
    {
        _jobRepository = jobRepository;
    }

    public async Task CloseJobsOpenTooLongAsync()
    {
        var cutoff = DateTime.UtcNow.AddDays(-MaxOpenDays);
        var staleJobs = await _jobRepository.GetActiveJobsCreatedBeforeAsync(cutoff);

        if (staleJobs.Count == 0)
        {
            return;
        }

        foreach (var job in staleJobs)
        {
            job.Status = JobStatus.Canceleld;
            _jobRepository.Update(job);
            Console.WriteLine(
                $"[AUTO-CLOSE] Job #{job.Id} \"{job.Title}\" was closed automatically (open more than {MaxOpenDays} days).");
        }

        await _jobRepository.SaveChangesAsync();
    }
}
