using Riok.Mapperly.Abstractions;
using StellarElysium.Domain.Dtos.ContaGenshin;
using StellarElysium.Domain.Dtos.Desejos.Consulta;
using StellarElysium.Domain.Dtos.Desejos.Request;
using StellarElysium.Domain.Entities;

namespace StellarElysium.Infrastructure.Mappers;

[Mapper]
public partial class DesejosMapper
{
    [MapperIgnoreTarget(nameof(ContaGenshin.Id))]
    [MapperIgnoreTarget(nameof(ContaGenshin.Desejos))]
    [MapperIgnoreTarget(nameof(ContaGenshin.DataCriacao))]
    [MapperIgnoreTarget(nameof(ContaGenshin.DataAtualizacao))]
    public partial ContaGenshin Map(ContaGenshinRequest dto);

    [MapperIgnoreSource(nameof(ContaGenshin.Desejos))]
    [MapperIgnoreSource(nameof(ContaGenshin.DataCriacao))]
    [MapperIgnoreSource(nameof(ContaGenshin.DataAtualizacao))]
    public partial ContaGenshinResponse Map(ContaGenshin entity);

    [MapperIgnoreSource(nameof(Desejo.ContaGenshin))]
    [MapperIgnoreSource(nameof(Desejo.DataCriacao))]
    [MapperIgnoreSource(nameof(Desejo.DataAtualizacao))]
    public partial DesejoResponse Map(Desejo entity);
}
