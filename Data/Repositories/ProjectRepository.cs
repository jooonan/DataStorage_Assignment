using Data.Context;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ProjectRepository(DataContext context)
{
    private readonly DataContext _context = context;

    public async Task AddProjectAsync(ProjectEntity project)
    {
        project.ProjectId = await GenerateProjectIdAsync();
        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ProjectEntity>> GetAllProjectsAsync()
    {
        return await _context.Projects
            .Include(p => p.Customer)
            .Include(p => p.User)
            .Include(p => p.Status)
            .Include(p => p.Product)
            .ToListAsync();
    }

    public async Task<ProjectEntity?> GetProjectByIdAsync(string projectId)
    {
        return await _context.Projects
            .Include(p => p.Customer)
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.ProjectId == projectId);
    }

    public async Task UpdateProjectAsync(ProjectEntity project)
    {
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteProjectAsync(string projectId)
    {
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == projectId);
        if (project != null)
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }
    }

    //Help from ChatGPT how to structure project id to follow p-101, p-102 etc.
    private async Task<string> GenerateProjectIdAsync()
    {
        int counter = 101;
        string newId;
        bool exists;

        do
        {
            newId = $"P-{counter}";
            exists = await _context.Projects.AnyAsync(p => p.ProjectId == newId);
            counter++;
        }
        while (exists);

        return newId;
    }
}
