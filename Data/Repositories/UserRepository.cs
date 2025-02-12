using Data.Context;

namespace Data.Repositories;

public class UserRepository(DataContext context)
{
    private readonly DataContext _context = context;
}
