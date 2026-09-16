using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JobApp.Application.Interfaces;
using JobApp.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JobApp.Infrastructure;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public (string token, DateTime expiresAt) GenerateToken(User user)
    {
        var secretKey = _configuration["JwtSettings:Secret"] ?? "SuperSecretKeyForJwtTokenGeneration_Digitera123!";
        var issuer = _configuration["JwtSettings:Issuer"] ?? "JobApp";
        var audience = _configuration["JwtSettings:Audience"] ?? "JobAppUsers";
        var expiryMinutes = int.TryParse(_configuration["JwtSettings:ExpiryMinutes"], out var mins) ? mins : 60;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var expiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: creds
        );

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }
}
