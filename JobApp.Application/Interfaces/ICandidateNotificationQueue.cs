namespace JobApp.Application.Interfaces;

public interface ICandidateNotificationQueue
{
    void QueueApplicationSubmitted(string candidateEmail, string jobTitle);
    void QueueApplicationCancelled(string candidateEmail, string jobTitle);
}
