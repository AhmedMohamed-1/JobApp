using System.Security.Cryptography;
using System.Text;
using JobApp.Application.DTOs;
using JobApp.Application.Interfaces;
using JobApp.Domain.Entities;

namespace JobApp.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var existingUser = await _userRepository.ExistsByEmailAsync(dto.Email);
        if (existingUser)
        {
            throw new InvalidOperationException("User with this email already exists.");
        }

        CreatePasswordHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        var (token, expiresAt) = _jwtTokenGenerator.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            RefreshToken = refreshToken,
            Username = user.Username,
            Email = user.Email,
            ExpiresAt = expiresAt,
            RefreshTokenExpiresAt = user.RefreshTokenExpiryTime
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null)
        {
            throw new InvalidOperationException("Invalid email or password.");
        }

        if (!VerifyPasswordHash(dto.Password, user.PasswordHash, user.PasswordSalt))
        {
            throw new InvalidOperationException("Invalid email or password.");
        }

        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await _userRepository.SaveChangesAsync();

        var (token, expiresAt) = _jwtTokenGenerator.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            RefreshToken = refreshToken,
            Username = user.Username,
            Email = user.Email,
            ExpiresAt = expiresAt,
            RefreshTokenExpiresAt = user.RefreshTokenExpiryTime
        };
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null || user.RefreshToken != dto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            throw new InvalidOperationException("Invalid or expired refresh token.");
        }

        var newRefreshToken = GenerateRefreshToken();
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await _userRepository.SaveChangesAsync();

        var (token, expiresAt) = _jwtTokenGenerator.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            RefreshToken = newRefreshToken,
            Username = user.Username,
            Email = user.Email,
            ExpiresAt = expiresAt,
            RefreshTokenExpiresAt = user.RefreshTokenExpiryTime
        };
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        using var hmac = new HMACSHA512(storedSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(storedHash);
    }
}
