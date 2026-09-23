using Hangfire;
using JobApp.Application.Interfaces;

namespace JobApp.Infrastructure.BackgroundJobs;

public class HangfireCandidateNotificationQueue : ICandidateNotificationQueue
{
    private readonly IBackgroundJobClient _backgroundJobClient;

    public HangfireCandidateNotificationQueue(IBackgroundJobClient backgroundJobClient)
    {
        _backgroundJobClient = backgroundJobClient;
    }

    public void QueueApplicationSubmitted(string candidateEmail, string jobTitle)
    {
        _backgroundJobClient.Enqueue<CandidateNotificationJobs>(
            jobs => jobs.SendApplicationSubmittedEmailAsync(candidateEmail, jobTitle));
    }

    public void QueueApplicationCancelled(string candidateEmail, string jobTitle)
    {
        _backgroundJobClient.Enqueue<CandidateNotificationJobs>(
            jobs => jobs.SendApplicationCancelledEmailAsync(candidateEmail, jobTitle));
    }
}
