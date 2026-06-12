using System.ComponentModel.DataAnnotations;
using StellarElysium.Domain.Entities;
using StellarElysium.Domain.Enums.Wishes;

namespace StellarElysium.Domain.Entities.Wishes;

public class GenshinAccount : BaseEntity
{
    [Required]
    [MaxLength(20)]
    public string Uid { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Nickname { get; set; }

    [Required]
    public GenshinServer Server { get; set; }

    public ICollection<Wish> Wishes { get; set; } = new List<Wish>();
}
