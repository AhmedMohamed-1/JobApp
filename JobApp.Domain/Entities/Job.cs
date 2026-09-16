using JobApp.Domain.Enums;

namespace JobApp.Domain.Entities;

public class Job
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public JobStatus Status { get; set; } = JobStatus.UnderReview;
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
    public int CreatedById { get; set; }
    public User? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
