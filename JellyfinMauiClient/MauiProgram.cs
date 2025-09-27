using CommunityToolkit.Mvvm;
using JellyfinMauiClient.Services;
using JellyfinMauiClient.ViewModels;

namespace JellyfinMauiClient;

public static class MauiProgram
{
    private static IServiceProvider? _provider;

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>();

        // Services
        builder.Services.AddSingleton<HttpClient>();
        builder.Services.AddSingleton<IJellyfinApiService, JellyfinApiService>();
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<MainPage>();

        var app = builder.Build();
        _provider = app.Services;
        ServiceLocator.Initialize(_provider);
        return app;
    }

    public static IServiceProvider Services => _provider ?? throw new InvalidOperationException("App not built yet");
}

public static class ServiceLocator
{
    private static IServiceProvider? _services;
    public static void Initialize(IServiceProvider services) => _services = services;
    public static T Resolve<T>() where T : notnull => _services is null
        ? throw new InvalidOperationException("Services not initialized")
        : _services.GetRequiredService<T>();
}
