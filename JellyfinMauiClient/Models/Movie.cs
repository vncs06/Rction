namespace JellyfinMauiClient.Models;

public sealed class Movie
{
    public string Id { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public int? ProductionYear { get; init; }
}

public sealed class JellyfinItemsResponse
{
    public List<JellyfinItem> Items { get; init; } = new();
}

public sealed class JellyfinItem
{
    public string Id { get; init; } = string.Empty;
    public string? Name { get; init; }
    public int? ProductionYear { get; init; }
    public string? Type { get; init; }
}
