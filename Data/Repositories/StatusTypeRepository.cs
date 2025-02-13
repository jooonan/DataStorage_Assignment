using Data.Context;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class StatusTypeRepository(DataContext context)
{
    private readonly DataContext _context = context;

    public async Task<IEnumerable<StatusTypeEntity>> GetAllStatusTypesAsync()
    {
        return await _context.StatusTypes.ToListAsync();
    }

    public async Task<StatusTypeEntity?> GetStatusTypeByIdAsync(int id)
    {
        return await _context.StatusTypes.FindAsync(id);
    }
}