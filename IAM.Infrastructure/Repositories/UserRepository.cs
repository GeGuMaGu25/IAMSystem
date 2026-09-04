using IAM.Domain.Entities;
using IAM.Domain.Repositories;
using IAM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IAM.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _Context;
    
    public UserRepository(ApplicationDbContext context)
    {
        _Context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _Context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task AddAsync(User user)
    {
        await _Context.Users.AddAsync(user);
        await _Context.SaveChangesAsync();
    }
}