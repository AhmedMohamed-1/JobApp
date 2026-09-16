using JobApp.Domain.Entities;

namespace JobApp.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(int id);
    Task<bool> ExistsByEmailAsync(string email);
    Task AddAsync(User user);
    Task SaveChangesAsync();
}
