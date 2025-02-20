using Data.Context;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class UserRepository(DataContext context)
{
    private readonly DataContext _context = context;

    public async Task AddUserAsync(UserEntity user)
    {
        try
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while adding user: {ex.Message}");
            throw;
        }
    }

    public async Task<UserEntity?> GetUserByIdAsync(int id)
    {
        try
        {
            return await _context.Users.FindAsync(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while retrieving user by ID ({id}): {ex.Message}");
            return null;
        }
    }

    public async Task<UserEntity?> GetUserByNameAsync(string firstName, string lastName)
    {
        try
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.FirstName == firstName && u.LastName == lastName);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while retrieving user by name ({firstName} {lastName}): {ex.Message}");
            return null;
        }
    }

    public async Task UpdateUserAsync(UserEntity user)
    {
        try
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while updating user (ID: {user.Id}): {ex.Message}");
            throw;
        }
    }
}
