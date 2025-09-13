using Kardamon.Factory;
using Kardamon.Services;

namespace Kardamon.ViewModels;

public partial class NavigationMenuViewModel : ViewModelBase
{
    private readonly PageFactory  _pageFactory;
    private readonly NavigationService _navigationService;
    private readonly SettingsService _settingsService;
    [ObservableProperty] private bool _isOpen;
    [ObservableProperty] ObservableCollection<IPage>? _pages;

    public NavigationMenuViewModel(NavigationService navigationService, PageFactory pageFactory, SettingsService settingsService)
    {
        _navigationService = navigationService;
        _pageFactory = pageFactory;
        _settingsService = settingsService;
    }
    
    public void Init()
    {
        _navigationService.OnNavigated += NavigationServiceOnOnNavigated;
        var pages = _pageFactory.GetPages().OrderBy(x=>x.Name);
        Pages = new ObservableCollection<IPage>(pages);
        _navigationService.NavigateTo(_pageFactory.GetExplorePage());
    }

    private void NavigationServiceOnOnNavigated()
    {
        ReturnToIndexCommand?.NotifyCanExecuteChanged();
    }

    [RelayCommand]
    private void ToggleIsOpen() => IsOpen = !IsOpen;

    private bool CanReturnToIndex() => _navigationService.CurrentPage is not IndexPageViewModel;
    [RelayCommand(CanExecute = "CanReturnToIndex")]
    private void ReturnToIndex()
    {
        _navigationService.GoToIndex();
    }

    [RelayCommand]
    private void OpenPage(IPage page)
    {
        _navigationService.NavigateTo(page);

        var isMobile = bool.Parse(_settingsService.GetSetting("IsMobile"));
        if(isMobile)
            if (IsOpen)
                IsOpen = false;

    }
}