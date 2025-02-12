using Data.Context;

namespace Data.Repositories;

public class ProjectRepository(DataContext context)
{
    private readonly DataContext _context = context;
}
