using DAL.Context;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class UserRepository : GenericRepository<User>
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public User? GetByEmail(string email)
    {
        return _context.Users
            .Include(u => u.Role)
            .FirstOrDefault(u => u.Email == email);
    }

    public User? GetWithRoleAsync(int id)
    {
        return _context.Users
            .Include(u => u.Role)
            .FirstOrDefault(u => u.Id == id);
    }
}