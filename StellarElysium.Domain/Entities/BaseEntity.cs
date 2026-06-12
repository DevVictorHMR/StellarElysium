using System.ComponentModel.DataAnnotations;

namespace StellarElysium.Domain.Entities;

public class BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
