using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class ProjectEntity
{
    [Key]
    public string? ProjectId { get; set; }

    [Required]
    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    [Column(TypeName = "date")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "date")]
    public DateTime EndDate { get; set; }


    [Required]
    [ForeignKey(nameof(Customer))]
    public int CustomerId { get; set; }
    public CustomerEntity Customer { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    public UserEntity User { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(Status))]
    public int StatusId { get; set; }
    public StatusTypeEntity Status { get; set; } = null!;

    [ForeignKey(nameof(Product))]
    public int ProductId { get; set; }
    public ProductEntity Product { get; set; } = null!;
}