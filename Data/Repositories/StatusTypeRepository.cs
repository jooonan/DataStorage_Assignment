using Data.Context;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class StatusTypeRepository(DataContext context)
{
    private readonly DataContext _context = context;

    public async Task<IEnumerable<StatusTypeEntity>> GetAllStatusTypesAsync()
    {
        try
        {
            return await _context.StatusTypes.ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while retrieving all status types: {ex.Message}");
            return [];
        }
    }

    public async Task<StatusTypeEntity?> GetStatusTypeByIdAsync(int id)
    {
        try
        {
            return await _context.StatusTypes.FindAsync(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while retrieving status type by ID ({id}): {ex.Message}");
            return null;
        }
    }
}
