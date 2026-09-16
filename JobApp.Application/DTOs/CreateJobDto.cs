using System.ComponentModel.DataAnnotations;

namespace JobApp.Application.DTOs;

public class CreateJobDto
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public string Company { get; set; } = string.Empty;

    [Required]
    public string Location { get; set; } = string.Empty;
}
