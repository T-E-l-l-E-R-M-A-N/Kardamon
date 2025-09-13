using Kardamon.Services;

namespace Kardamon.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly NavigationService _navigationService;
    [ObservableProperty] NavigationMenuViewModel _navigationMenu;
    [ObservableProperty] CommandBarViewModel _commandBar;
    [ObservableProperty] MiniPlayerViewModel _miniPlayer;
    [ObservableProperty] private ToastNotificationViewModel _toastNotification;
    [ObservableProperty] IPage? _currentPage;

    public MainViewModel(NavigationMenuViewModel navigationMenu, NavigationService navigationService, CommandBarViewModel commandBar, MiniPlayerViewModel miniPlayer, ToastNotificationViewModel toastNotification)
    {
        _navigationMenu = navigationMenu;
        _navigationService = navigationService;
        _commandBar = commandBar;
        _miniPlayer = miniPlayer;
        _toastNotification = toastNotification;
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
}