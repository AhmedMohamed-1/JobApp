namespace JobApp.Infrastructure.BackgroundJobs;

public class CandidateNotificationJobs
{
    public Task SendApplicationSubmittedEmailAsync(string candidateEmail, string jobTitle)
    {
        Console.WriteLine(
            $"[EMAIL SIMULATION] To: {candidateEmail} | Subject: Application received | You have successfully applied to \"{jobTitle}\".");
        return Task.CompletedTask;
    }

    public Task SendApplicationCancelledEmailAsync(string candidateEmail, string jobTitle)
    {
        Console.WriteLine(
            $"[EMAIL SIMULATION] To: {candidateEmail} | Subject: Application cancelled | Your application for \"{jobTitle}\" has been cancelled.");
        return Task.CompletedTask;
    }
}
