using StellarElysium.Domain.Enums;

namespace StellarElysium.Domain.Dtos.ContaGenshin;

public class ContaGenshinResponse
{
    public Guid Id { get; set; }
    public string Uid { get; set; } = string.Empty;
    public string? Apelido { get; set; }
    public ServidorGenshin Servidor { get; set; }
}
