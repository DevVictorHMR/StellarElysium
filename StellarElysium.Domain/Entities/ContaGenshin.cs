using System.ComponentModel.DataAnnotations;
using StellarElysium.Domain.Enums;

namespace StellarElysium.Domain.Entities;

public class ContaGenshin : EntidadeBase
{
    [Required]
    [MaxLength(20)]
    public string Uid { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Apelido { get; set; }

    [Required]
    public ServidorGenshin Servidor { get; set; }

    public ICollection<Desejo> Desejos { get; set; } = new List<Desejo>();
}
