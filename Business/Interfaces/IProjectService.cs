using Business.Dtos;

namespace Business.Interfaces;

public interface IProjectService
{
    Task<ProjectDto> AddProjectAsync(ProjectDto projectDto);
    Task DeleteProjectAsync(string projectId);
    Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();
    Task<ProjectDto?> GetProjectByIdAsync(string projectId);
    Task UpdateProjectAsync(ProjectDto projectDto);
}