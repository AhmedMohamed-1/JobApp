using System.ComponentModel.DataAnnotations;

namespace JobApp.Application.DTOs;

public class ApplyJobDto
{
    [Required]
    [Url]
    public string CvLink { get; set; } = string.Empty;
}
