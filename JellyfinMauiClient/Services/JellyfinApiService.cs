using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using JellyfinMauiClient.Models;

namespace JellyfinMauiClient.Services;

public interface IJellyfinApiService
{
    Task<IReadOnlyList<Movie>> GetUserMoviesAsync(string userId, CancellationToken ct = default);
}

public sealed class JellyfinApiService(HttpClient http) : IJellyfinApiService
{
    // Replace placeholders below
    private const string BaseUrl = "http://localhost:8096"; // e.g., https://jellyfin.example.com:8096
    private const string ApiKey = "a1327e4b98c141bb9f2cd6a10eb3f7e6";
    

    public async Task<IReadOnlyList<Movie>> GetUserMoviesAsync(string userId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException("UserId is required", nameof(userId));
        if (BaseUrl == "YOUR_SERVER_URL" || !Uri.TryCreate(BaseUrl, UriKind.Absolute, out var baseUri) || (baseUri.Scheme != Uri.UriSchemeHttp && baseUri.Scheme != Uri.UriSchemeHttps))
            throw new InvalidOperationException("Configure YOUR_SERVER_URL in Services/JellyfinApiService.cs (ex.: https://host:8096).");
        if (string.IsNullOrWhiteSpace(ApiKey) || ApiKey == "YOUR_API_KEY")
            throw new InvalidOperationException("Configure YOUR_API_KEY em Services/JellyfinApiService.cs (X-Emby-Token).");

        using var req = new HttpRequestMessage(HttpMethod.Get, $"{baseUri.ToString().TrimEnd('/')}/Users/{Uri.EscapeDataString(userId)}/Items?IncludeItemTypes=Movie&Recursive=true&Fields=ProductionYear");
        req.Headers.Add("X-Emby-Token", ApiKey);

        using var res = await http.SendAsync(req, ct).ConfigureAwait(false);
        res.EnsureSuccessStatusCode();

        var json = await res.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var dto = JsonSerializer.Deserialize<JellyfinItemsResponse>(json, options) ?? new JellyfinItemsResponse();

        var movies = dto.Items
            .Where(i => string.Equals(i.Type, "Movie", StringComparison.OrdinalIgnoreCase))
            .Select(i => new Movie
            {
                Id = i.Id,
                Title = i.Name ?? string.Empty,
                ProductionYear = i.ProductionYear
            })
            .OrderBy(m => m.Title)
            .ToList();

        return movies;
    }
}
