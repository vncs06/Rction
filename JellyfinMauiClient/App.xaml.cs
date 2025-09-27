using JellyfinMauiClient.Services;
using JellyfinMauiClient.ViewModels;

namespace JellyfinMauiClient;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // MainPage resolved via DI; BindingContext is provided by DI in page constructor or OnAppearing
        var page = ServiceLocator.Resolve<MainPage>();
        page.BindingContext ??= ServiceLocator.Resolve<MainViewModel>();
        MainPage = new NavigationPage(page);
    }
}
