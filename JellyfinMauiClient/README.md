# Jellyfin MAUI Client (Starter)

Minimal .NET MAUI app using MVVM to list a user's movies from a Jellyfin server.

## Configure
Edit the placeholders in the following files:
- `Services/JellyfinApiService.cs`: set `BaseUrl` and `ApiKey`
- `ViewModels/MainViewModel.cs`: set `UserId`

Placeholders:
- `YOUR_SERVER_URL` (e.g., `https://your-host:8096`)
- `YOUR_API_KEY`
- `YOUR_USER_ID`

## Build and Run (Windows)
- Requires .NET 8 SDK and MAUI workload.
- In VS Code or Visual Studio, restore and run the Windows target.

## Notes
- Uses header `X-Emby-Token` for API key auth.
- Calls `GET /Users/{userId}/Items?IncludeItemTypes=Movie&Recursive=true&Fields=ProductionYear`.
- Displays Title and ProductionYear in a simple list.