using Business.Dtos;
using Business.Interfaces;
using Data.Entities;
using Data.Repositories;

namespace Business.Services;

public class ProjectService(ProjectRepository projectRepository) : IProjectService
{
    private readonly ProjectRepository _projectRepository = projectRepository;

    public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
    {
        var projects = await _projectRepository.GetAllProjectsAsync();
        return projects.Select(p => new ProjectDto
        {
            ProjectId = p.ProjectId,
            Title = p.Title,
            Description = p.Description,
            StartDate = p.StartDate,
            EndDate = p.EndDate,
            CustomerId = p.CustomerId,
            UserId = p.UserId,
            StatusId = p.StatusId,
            ProductId = p.ProductId
        }).ToList();
    }

    public async Task<ProjectDto?> GetProjectByIdAsync(string projectId)
    {
        var project = await _projectRepository.GetProjectByIdAsync(projectId);
        if (project == null) return null;

        return new ProjectDto
        {
            ProjectId = project.ProjectId,
            Title = project.Title,
            Description = project.Description,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            CustomerId = project.CustomerId,
            UserId = project.UserId,
            StatusId = project.StatusId,
            ProductId = project.ProductId
        };
    }

    public async Task<ProjectDto> AddProjectAsync(ProjectDto projectDto)
    {
        var projectEntity = new ProjectEntity
        {
            ProjectId = projectDto.ProjectId,
            Title = projectDto.Title,
            Description = projectDto.Description,
            StartDate = projectDto.StartDate,
            EndDate = projectDto.EndDate,
            CustomerId = projectDto.CustomerId,
            UserId = projectDto.UserId,
            StatusId = projectDto.StatusId,
            ProductId = projectDto.ProductId
        };

        await _projectRepository.AddProjectAsync(projectEntity);

        return new ProjectDto
        {
            ProjectId = projectEntity.ProjectId,
            Title = projectEntity.Title,
            Description = projectEntity.Description,
            StartDate = projectEntity.StartDate,
            EndDate = projectEntity.EndDate,
            CustomerId = projectEntity.CustomerId,
            UserId = projectEntity.UserId,
            StatusId = projectEntity.StatusId,
            ProductId = projectEntity.ProductId
        };
    }

    public async Task UpdateProjectAsync(ProjectDto projectDto)
    {
        var existingProject = await _projectRepository.GetProjectByIdAsync(projectDto.ProjectId) ?? throw new Exception("Project not found.");
        existingProject.Title = projectDto.Title;
        existingProject.Description = projectDto.Description;
        existingProject.StartDate = projectDto.StartDate;
        existingProject.EndDate = projectDto.EndDate != DateTime.MinValue ? projectDto.EndDate : existingProject.EndDate;
        existingProject.CustomerId = projectDto.CustomerId;
        existingProject.UserId = projectDto.UserId;
        existingProject.ProductId = projectDto.ProductId;
        existingProject.StatusId = projectDto.StatusId;

        await _projectRepository.UpdateProjectAsync(existingProject);
    }

    public async Task DeleteProjectAsync(string projectId)
    {
        await _projectRepository.DeleteProjectAsync(projectId);
    }
}
