using JobApp.Domain.Enums;

namespace JobApp.Application.DTOs;

public class JobApplicationResponseDto
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public int ApplicantId { get; set; }
    public string CvLink { get; set; } = string.Empty;
    public JobStatus Status { get; set; }
    public string StatusName => Status.ToString();
    public DateTime AppliedAt { get; set; }
}
