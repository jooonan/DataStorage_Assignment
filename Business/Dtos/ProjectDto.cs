using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class ProjectDto
{
    public string ProjectId { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int CustomerId { get; set; }
    public int UserId { get; set; }
    public int StatusId { get; set; }
    public int ProductId { get; set; }
}