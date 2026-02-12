using System.ComponentModel.DataAnnotations;

namespace StellarElysium.Domain.Entities;

public class EntidadeBase
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public DateTime DataCriacao { get; set; }

    public DateTime? DataAtualizacao { get; set; }
}
