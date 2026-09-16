using JobApp.Domain.Entities;

namespace JobApp.Application.Interfaces;

public interface IJwtTokenGenerator
{
    (string token, DateTime expiresAt) GenerateToken(User user);
}
