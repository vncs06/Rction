using JellyfinMauiClient.ViewModels;

namespace JellyfinMauiClient;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is MainViewModel vm)
        {
            await vm.LoadCommand.ExecuteAsync(null);
        }
    }
}
