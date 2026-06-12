using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using StellarElysium.Application.Interfaces.Services.Wishes;
using StellarElysium.Domain.Dtos.Wishes.Import;
using StellarElysium.Domain.Enums.Localization;
using StellarElysium.Domain.Exceptions;

namespace StellarElysium.Infrastructure.Services.Wishes;

public sealed class WishHistoryService(
    HttpClient httpClient,
    ILogger<WishHistoryService> logger) : IWishHistoryService
{
    private const int PageSize = 20;
    private static readonly IReadOnlyList<int> DefaultGachaTypes = [100, 200, 301, 302, 400];

    public async Task<IReadOnlyList<WishImportItemRequest>> FetchAsync(
        string url,
        IReadOnlyCollection<int> gachaTypes,
        CancellationToken cancellationToken)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
        {
            throw new LocalizedApiException(ApiMessageKey.InvalidHistoryUrl);
        }

        var baseUri = new UriBuilder(uri);
        if (!baseUri.Path.Contains("getGachaLog", StringComparison.OrdinalIgnoreCase))
        {
            baseUri.Path = "/gacha_info/api/getGachaLog";
        }

        var baseParameters = ParseQuery(baseUri.Query);
        if (!baseParameters.TryGetValue("authkey", out var authkey) || string.IsNullOrWhiteSpace(authkey))
        {
            throw new LocalizedApiException(ApiMessageKey.HistoryUrlMissingAuthkey);
        }

        if (string.IsNullOrWhiteSpace(GetParameter(baseParameters, "lang")))
        {
            baseParameters["lang"] = "en-us";
        }

        var types = gachaTypes.Count > 0 ? gachaTypes : DefaultGachaTypes;
        var result = new List<WishImportItemRequest>();

        foreach (var gachaType in types)
        {
            logger.LogInformation("Fetching wish history. GachaType {GachaType}", gachaType);

            var page = 1;
            var endId = "0";

            while (true)
            {
                var parameters = new Dictionary<string, string>(baseParameters, StringComparer.OrdinalIgnoreCase)
                {
                    ["gacha_type"] = gachaType.ToString(CultureInfo.InvariantCulture),
                    ["init_type"] = gachaType.ToString(CultureInfo.InvariantCulture),
                    ["page"] = page.ToString(CultureInfo.InvariantCulture),
                    ["size"] = PageSize.ToString(CultureInfo.InvariantCulture),
                    ["end_id"] = endId
                };

                baseUri.Query = BuildQuery(parameters);

                var response = await httpClient.GetFromJsonAsync<GachaLogResponse>(baseUri.Uri, cancellationToken);
                if (response is null)
                {
                    logger.LogWarning("Empty response while fetching wish history. GachaType {GachaType}", gachaType);
                    break;
                }

                if (response.Retcode != 0)
                {
                    logger.LogWarning("Error while fetching wish history. Retcode {Retcode} Message {Message}", response.Retcode, response.Message);
                    break;
                }

                var items = response.Data?.List ?? [];
                if (items.Count == 0)
                {
                    break;
                }

                foreach (var item in items)
                {
                    result.Add(new WishImportItemRequest
                    {
                        PullId = item.Id,
                        GachaType = item.GachaType,
                        ItemId = item.ItemId,
                        Quantity = item.Count,
                        WishTime = item.Time,
                        ItemName = item.Name,
                        ItemType = item.ItemType,
                        RankType = item.RankType,
                        Language = item.Lang
                    });
                }

                endId = items[^1].Id;
                page++;
            }
        }

        logger.LogInformation("Wish history fetched. Total {Total}", result.Count);
        return result;
    }

    private static string? GetParameter(Dictionary<string, string> parameters, string key)
    {
        return parameters.TryGetValue(key, out var value) ? value : null;
    }

    private static Dictionary<string, string> ParseQuery(string query)
    {
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var withoutPrefix = query.StartsWith("?") ? query[1..] : query;

        foreach (var part in withoutPrefix.Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            var index = part.IndexOf('=', StringComparison.Ordinal);
            if (index <= 0)
            {
                result[Uri.UnescapeDataString(part)] = string.Empty;
                continue;
            }

            var key = Uri.UnescapeDataString(part[..index]);
            var value = Uri.UnescapeDataString(part[(index + 1)..]);
            result[key] = value;
        }

        return result;
    }

    private static string BuildQuery(Dictionary<string, string> parameters)
    {
        return string.Join("&", parameters.Select(item =>
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
