using System.ComponentModel.DataAnnotations;
using StellarElysium.Domain.Enums;

namespace StellarElysium.Domain.Dtos.ContaGenshin;

public class ContaGenshinRequest
{
    [Required]
    [MaxLength(20)]
    public string Uid { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Apelido { get; set; }

    [Required]
    public ServidorGenshin Servidor { get; set; }
}
