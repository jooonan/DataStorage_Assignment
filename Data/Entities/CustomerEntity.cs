using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class CustomerEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string CustomerName { get; set; } = string.Empty;

    public ICollection<ProjectEntity> Projects { get; set; } = [];
}