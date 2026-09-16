using JobApp.Application.Interfaces;
using JobApp.Domain.Entities;
using JobApp.Infrastructure.Presistence;
using Microsoft.EntityFrameworkCore;

namespace JobApp.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
