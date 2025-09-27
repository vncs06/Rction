using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JellyfinMauiClient.Models;
using JellyfinMauiClient.Services;

namespace JellyfinMauiClient.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IJellyfinApiService _api;

    // Replace with your user id or bind via settings
    private const string UserId = "f96df2511ecd48fcb76ca87fec4c5836";

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    public ObservableCollection<Movie> Movies { get; } = new();

    public MainViewModel(IJellyfinApiService api)
    {
        _api = api;
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            ErrorMessage = null;
            Movies.Clear();
            if (UserId == "YOUR_USER_ID")
                throw new InvalidOperationException("Configure YOUR_USER_ID em ViewModels/MainViewModel.cs.");

            var result = await _api.GetUserMoviesAsync(UserId);
            foreach (var m in result)
                Movies.Add(m);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }
}
