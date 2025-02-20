using Data.Context;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ProjectRepository(DataContext context)
{
    private readonly DataContext _context = context;

    public async Task AddProjectAsync(ProjectEntity project)
    {
        try
        {
            project.ProjectId = await GenerateProjectIdAsync();
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while adding project: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<ProjectEntity>> GetAllProjectsAsync()
    {
        try
        {
            return await _context.Projects
                .Include(p => p.Customer)
                .Include(p => p.User)
                .Include(p => p.Status)
                .Include(p => p.Product)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while retrieving all projects: {ex.Message}");
            return [];
        }
    }

    public async Task<ProjectEntity?> GetProjectByIdAsync(string projectId)
    {
        try
        {
            return await _context.Projects
                .Include(p => p.Customer)
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.ProjectId == projectId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while retrieving project by ID ({projectId}): {ex.Message}");
            return null;
        }
    }

    public async Task UpdateProjectAsync(ProjectEntity project)
    {
        try
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while updating project (ID: {project.ProjectId}): {ex.Message}");
            throw;
        }
    }

    public async Task DeleteProjectAsync(string projectId)
    {
        try
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == projectId);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while deleting project (ID: {projectId}): {ex.Message}");
            throw;
        }
    }

    // Help from ChatGPT how to structure project id to follow P-101, P-102 etc.
    private async Task<string> GenerateProjectIdAsync()
    {
        try
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
        catch (Exception ex)
        {
            Console.WriteLine($"Error while generating project ID: {ex.Message}");
            throw;
        }
    }
}
