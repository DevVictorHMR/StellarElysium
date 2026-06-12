using System.ComponentModel.DataAnnotations;
using StellarElysium.Domain.Enums;
using StellarElysium.Domain.Enums.Wishes;

namespace StellarElysium.Domain.Dtos.Wishes.Query;

public record WishQueryRequest
{
    [Required]
    public Guid GenshinAccountId { get; set; }

    public int? GachaType { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public int? RankType { get; set; }

    [MaxLength(100)]
    public string? Name { get; set; }

    [MaxLength(30)]
    public string? ItemType { get; set; }

    public WishSortField? SortBy { get; set; }
    public SortDirection Direction { get; set; } = SortDirection.Desc;

    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 100;
}
