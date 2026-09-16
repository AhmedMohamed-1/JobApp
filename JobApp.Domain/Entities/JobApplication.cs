using JobApp.Domain.Enums;

namespace JobApp.Domain.Entities;

public class JobApplication
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public Job? Job { get; set; }
    public int ApplicantId { get; set; }
    public User? Applicant { get; set; }
    public string CvLink { get; set; } = string.Empty;
    public JobStatus Status { get; set; } = JobStatus.Applied;
    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
}
