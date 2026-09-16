using JobApp.Domain.Enums;

namespace JobApp.Application.DTOs;

public class JobResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public JobStatus Status { get; set; }
    public string StatusName => Status.ToString();
    public int CreatedById { get; set; }
    public DateTime CreatedAt { get; set; }
}
