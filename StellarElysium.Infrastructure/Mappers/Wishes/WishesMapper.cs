using Riok.Mapperly.Abstractions;
using StellarElysium.Domain.Dtos.Wishes.Accounts;
using StellarElysium.Domain.Dtos.Wishes.Query;
using StellarElysium.Domain.Dtos.Wishes.Requests;
using StellarElysium.Domain.Entities.Wishes;

namespace StellarElysium.Infrastructure.Mappers.Wishes;

[Mapper]
public partial class WishesMapper
{
    [MapperIgnoreTarget(nameof(GenshinAccount.Id))]
    [MapperIgnoreTarget(nameof(GenshinAccount.Wishes))]
    [MapperIgnoreTarget(nameof(GenshinAccount.CreatedAt))]
    [MapperIgnoreTarget(nameof(GenshinAccount.UpdatedAt))]
    public partial GenshinAccount Map(GenshinAccountRequest dto);

    [MapperIgnoreSource(nameof(GenshinAccount.Wishes))]
    [MapperIgnoreSource(nameof(GenshinAccount.CreatedAt))]
    [MapperIgnoreSource(nameof(GenshinAccount.UpdatedAt))]
    public partial GenshinAccountResponse Map(GenshinAccount entity);

    [MapperIgnoreSource(nameof(Wish.GenshinAccount))]
    [MapperIgnoreSource(nameof(Wish.CreatedAt))]
    [MapperIgnoreSource(nameof(Wish.UpdatedAt))]
    public partial WishResponse Map(Wish entity);
}
