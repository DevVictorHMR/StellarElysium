using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using StellarElysium.Application.Interfaces.Services;
using StellarElysium.Domain.Dtos.Desejos.Importacao;

namespace StellarElysium.Infrastructure.Services;

public sealed class DesejoHistoricoService(
    HttpClient httpClient,
    ILogger<DesejoHistoricoService> logger) : IDesejoHistoricoService
{
    private const int TamanhoPagina = 20;
    private static readonly IReadOnlyList<int> TiposPadrao = [100, 200, 301, 302, 400];

    public async Task<IReadOnlyList<DesejoImportacaoRequest>> BuscarAsync(
        string url,
        IReadOnlyCollection<int> gachaTypes,
        CancellationToken cancellationToken)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
        {
            throw new InvalidOperationException("Url de historico invalida.");
        }

        var baseUri = new UriBuilder(uri);
        if (!baseUri.Path.Contains("getGachaLog", StringComparison.OrdinalIgnoreCase))
        {
            baseUri.Path = "/gacha_info/api/getGachaLog";
        }

        var baseParametros = ParseQuery(baseUri.Query);
        if (!baseParametros.TryGetValue("authkey", out var authkey) || string.IsNullOrWhiteSpace(authkey))
        {
            throw new InvalidOperationException("Url de historico sem authkey.");
        }

        if (string.IsNullOrWhiteSpace(GetParametro(baseParametros, "lang")))
        {
            baseParametros["lang"] = "en-us";
        }

        var tipos = gachaTypes.Count > 0 ? gachaTypes : TiposPadrao;
        var resultado = new List<DesejoImportacaoRequest>();

        foreach (var gachaType in tipos)
        {
            logger.LogInformation("Buscando historico. GachaType {GachaType}", gachaType);

            var page = 1;
            var endId = "0";

            while (true)
            {
                var parametros = new Dictionary<string, string>(baseParametros, StringComparer.OrdinalIgnoreCase)
                {
                    ["gacha_type"] = gachaType.ToString(CultureInfo.InvariantCulture),
                    ["init_type"] = gachaType.ToString(CultureInfo.InvariantCulture),
                    ["page"] = page.ToString(CultureInfo.InvariantCulture),
                    ["size"] = TamanhoPagina.ToString(CultureInfo.InvariantCulture),
                    ["end_id"] = endId
                };

                baseUri.Query = BuildQuery(parametros);

                var response = await httpClient.GetFromJsonAsync<GachaLogResponse>(baseUri.Uri, cancellationToken);
                if (response is null)
                {
                    logger.LogWarning("Resposta vazia ao buscar historico. GachaType {GachaType}", gachaType);
                    break;
                }

                if (response.Retcode != 0)
                {
                    logger.LogWarning("Erro ao buscar historico. Retcode {Retcode} Mensagem {Mensagem}", response.Retcode, response.Message);
                    break;
                }

                var itens = response.Data?.List ?? [];
                if (itens.Count == 0)
                {
                    break;
                }

                foreach (var item in itens)
                {
                    resultado.Add(new DesejoImportacaoRequest
                    {
                        PullId = item.Id,
                        GachaType = item.GachaType,
                        ItemId = item.ItemId,
                        Quantidade = item.Count,
                        DataHora = item.Time,
                        NomeItem = item.Name,
                        TipoItem = item.ItemType,
                        RankType = item.RankType,
                        Idioma = item.Lang
                    });
                }

                endId = itens[^1].Id;
                page++;
            }
        }

        logger.LogInformation("Historico obtido. Total {Total}", resultado.Count);
        return resultado;
    }

    private static string? GetParametro(Dictionary<string, string> parametros, string chave)
    {
        return parametros.TryGetValue(chave, out var valor) ? valor : null;
    }

    private static Dictionary<string, string> ParseQuery(string query)
    {
        var resultado = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var semPrefixo = query.StartsWith("?") ? query[1..] : query;

        foreach (var parte in semPrefixo.Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            var index = parte.IndexOf('=', StringComparison.Ordinal);
            if (index <= 0)
            {
                resultado[Uri.UnescapeDataString(parte)] = string.Empty;
                continue;
            }

            var chave = Uri.UnescapeDataString(parte[..index]);
            var valor = Uri.UnescapeDataString(parte[(index + 1)..]);
            resultado[chave] = valor;
        }

        return resultado;
    }

    private static string BuildQuery(Dictionary<string, string> parametros)
    {
        return string.Join("&", parametros.Select(item =>
            $"{Uri.EscapeDataString(item.Key)}={Uri.EscapeDataString(item.Value)}"));
    }

    private sealed class GachaLogResponse
    {
        [JsonPropertyName("retcode")]
        public int Retcode { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public GachaLogData? Data { get; set; }
    }

    private sealed class GachaLogData
    {
        [JsonPropertyName("list")]
        public List<GachaLogItem> List { get; set; } = new();
    }

    private sealed class GachaLogItem
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("gacha_type")]
        public string GachaType { get; set; } = string.Empty;

        [JsonPropertyName("item_id")]
        public string? ItemId { get; set; }

        [JsonPropertyName("count")]
        public string Count { get; set; } = "1";

        [JsonPropertyName("time")]
        public string Time { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("item_type")]
        public string ItemType { get; set; } = string.Empty;

        [JsonPropertyName("rank_type")]
        public string RankType { get; set; } = string.Empty;

        [JsonPropertyName("lang")]
        public string? Lang { get; set; }
    }
}
