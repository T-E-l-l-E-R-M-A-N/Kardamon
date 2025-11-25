using Kardamon.Services;

namespace Kardamon.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly NavigationService _navigationService;
    private readonly WebSearchService _webSearchService;
    
    [ObservableProperty] NavigationMenuViewModel _navigationMenu;
    [ObservableProperty] CommandBarViewModel _commandBar;
    [ObservableProperty] MiniPlayerViewModel _miniPlayer;
    [ObservableProperty] private ToastNotificationViewModel _toastNotification;
    [ObservableProperty] IPage? _currentPage;
    [ObservableProperty] private bool _nowPlayingModalIsPresented;

    public MainViewModel(NavigationMenuViewModel navigationMenu, NavigationService navigationService, CommandBarViewModel commandBar, MiniPlayerViewModel miniPlayer, ToastNotificationViewModel toastNotification, WebSearchService webSearchService)
    {
        _navigationMenu = navigationMenu;
        _navigationService = navigationService;
        _commandBar = commandBar;
        _miniPlayer = miniPlayer;
        _toastNotification = toastNotification;
        _webSearchService = webSearchService;
    }

    public void Init()
    {
        _navigationService.OnNavigated += NavigationServiceOnOnNavigated;
        ToastNotification.Init();
        NavigationMenu.Init();
    }

    private void NavigationServiceOnOnNavigated()
    {
        CurrentPage = _navigationService.CurrentPage;
    }

    [RelayCommand]
    private void ToggleNowPlayingModal()
    {
        _navigationService.GoToNowPlaying();
    }
    
    [RelayCommand]
    private async Task Play(SongModel s)
    {
        await _miniPlayer.PlaySingleAsync(s, true);
    }
    [RelayCommand]
    private async Task MarkFavorite(SongModel s)
    {
        await _webSearchService.ChangeFavorite(s);
        _navigationService.GoToFavorites();


    }
}