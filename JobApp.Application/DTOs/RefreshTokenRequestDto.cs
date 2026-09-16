using System.ComponentModel.DataAnnotations;

namespace JobApp.Application.DTOs;

public class RefreshTokenRequestDto
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;
}
