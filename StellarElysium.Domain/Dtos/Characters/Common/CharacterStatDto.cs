namespace StellarElysium.Domain.Dtos.Characters.Common;

public record CharacterStatDto
{
    public int Level { get; set; }
    public int AscensionPhase { get; set; }
    public bool IsAscended { get; set; }
    public decimal Hp { get; set; }
    public decimal Attack { get; set; }
    public decimal Defense { get; set; }
    public decimal CritRatePercent { get; set; }
    public decimal CritDamagePercent { get; set; }
    public string? BonusAttribute { get; set; }
    public decimal? BonusValuePercent { get; set; }
}
